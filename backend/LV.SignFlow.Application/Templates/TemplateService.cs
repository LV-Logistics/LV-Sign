using LV.SignFlow.Application.Common.Interfaces;
using LV.SignFlow.Application.Templates.Dtos;
using LV.SignFlow.Application.Templates.Requests;
using LV.SignFlow.Domain.Entities.Envelopes;
using LV.SignFlow.Domain.Entities.Templates;
using LV.SignFlow.Domain.Entities.Users;
using LV.SignFlow.Domain.Enums.TemplateEnums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using static LV.SignFlow.Domain.Authorization.PermissionCodes;

namespace LV.SignFlow.Application.Templates
{
    public class TemplateService : ITemplateService
    {
        private readonly ITemplateRepository _templateRepository;
        private readonly ICurrentUser _currentUser;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileStorage _fileStorage;
        private readonly IRepository<TemplateRecipientRole> _recipientRoleRepository;
        private readonly IRepository<TemplateDocument> _templateDocumentRepository;
        private readonly IRepository<TemplateField> _templateFieldRepository;
        private readonly IRepository<User> _userRepository;

        public TemplateService(ITemplateRepository templateRepository, ICurrentUser curentUser, IUnitOfWork unitOfWork, IFileStorage fileStorage, IRepository<TemplateRecipientRole> recipientRoleRepository, IRepository<TemplateDocument> templateDocumentRepository, IRepository<User> userRepository, IRepository<TemplateField> templateFieldRepository)
        {
            _templateRepository = templateRepository;
            _currentUser = curentUser;
            _unitOfWork = unitOfWork;
            _fileStorage = fileStorage;
            _recipientRoleRepository = recipientRoleRepository;
            _templateDocumentRepository = templateDocumentRepository;
            _userRepository = userRepository;
            _templateFieldRepository = templateFieldRepository;
        }

        private void EnsureAuthenticated()
        {
            if (!_currentUser.IsAuthentificated)
            {
                throw new UnauthorizedAccessException("Authentification is required");
            }
        }
        public async Task<Guid> CreateAsync(CreateTemplateRequest request, CancellationToken cancellationToken = default)
        {
            EnsureAuthenticated();

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                throw new ArgumentException("Template name is required", nameof(request));
            }

            var now = DateTimeOffset.UtcNow;

            var template = new Template
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                OrganizationId = _currentUser.OrganizationId,
                OwnerUserId = _currentUser.UserId,
                Status = TemplateStatus.Draft,
                IsDeleted = false,
                CreatedAt = now,
                UpdatedAt = now
            };
            var version = new TemplateVersion
            {
                Id = Guid.NewGuid(),
                CreatedByUserId = _currentUser.UserId,
                TemplateId = template.Id,
                Template = template,
                CreatedAt = now,
                IsPublished = false,
                VersionNumber = 1
            };

            template.Versions.Add(version);
            await _templateRepository.AddAsync(template, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return template.Id;
        }

        public async Task<IReadOnlyList<TemplateListItemDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            EnsureAuthenticated();
            var templates = await _templateRepository.GetAllForOrganizationAsync(_currentUser.OrganizationId, cancellationToken);

            return templates.Select(x => new TemplateListItemDto
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                Status = x.Status,
                LatestVersionNumber = x.Versions.Count == 0 ? 0 : x.Versions.Max(x => x.VersionNumber),
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
            }).ToList();
        }

        public async Task<TemplateDetailsDto?> GetByIdAsync(Guid Id, CancellationToken cancellationToken = default)
        {
            EnsureAuthenticated();
            var template = await _templateRepository.GetByIdForOrganizationAsync(Id, _currentUser.OrganizationId, cancellationToken);
            if (template is null)
            {
                return null;
            }

            return new TemplateDetailsDto
            {
                Id = template.Id,
                Name = template.Name,
                Description = template.Description,
                CreatedAt = template.CreatedAt,
                UpdatedAt = template.UpdatedAt,
                OwnerUserId = template.OwnerUserId,
                LatestVersionNumber = template.Versions.Count == 0 ? 0 :
                template.Versions.Max(x => x.VersionNumber),

            };
        }

        public async Task<IReadOnlyList<TemplateRecipientRoleDto>> GetRecipientRoleAsync(Guid templateId, CancellationToken cancellationToken = default)
        {
            EnsureAuthenticated();
            var templates = await _templateRepository.GetByIdForOrganizationAsync(templateId, _currentUser.OrganizationId, cancellationToken);
            if (templates is null) throw new KeyNotFoundException("template was not found");
            var latestVersion = templates.Versions.OrderByDescending(x => x.VersionNumber).FirstOrDefault();

            if (latestVersion is null)
                throw new InvalidOperationException(
                    "The template does not contain a version.");

            var roles = await _recipientRoleRepository.FindAsync(x => x.TemplateVersionId == latestVersion.Id, cancellationToken);

            return roles.OrderBy(x => x.RoutingOrder).Select(x => new TemplateRecipientRoleDto
            {
                Id = x.Id,
                Name = x.Name,
                TemplateVersionId = x.TemplateVersionId,
                RecipientType = x.RecipientType,
                RoutingOrder = x.RoutingOrder,
                IsRequired = x.IsRequired,
                DefaultUserId = x.DefaultUserId,
                AllowSenderEditRecipient = x.AllowSenderEditRecipient,
                AllowSenderDeleteRecipient = x.AllowSenderDeleteRecipient,
                AllowSenderChangeRoutingOrder = x.AllowSenderChangeRoutingOrder
            }).ToList();
        }

        public async Task<TemplateRecipientRoleDto> AddRecipientRoleAsync(Guid templateId, TemplateRecipientRoleRequest request, CancellationToken cancellationToken = default)
        {
            EnsureAuthenticated();
            if (string.IsNullOrWhiteSpace(request.Name))
                throw new ArgumentException(
                    "Recipient role name is required.");

            if (request.RoutingOrder <= 0)
                throw new ArgumentException(
                    "Routing order must be greater than zero.");

            if (!request.AllowSenderEditRecipient &&
                request.DefaultUserId is null)
            {
                throw new ArgumentException(
                    "A locked recipient must have a default user.");
            }

            var template =
      await _templateRepository.GetByIdForOrganizationAsync(
          templateId,
          _currentUser.OrganizationId,
          cancellationToken);

            if (template is null)
                throw new KeyNotFoundException(
                    "Template was not found.");

            if (template.Status != TemplateStatus.Draft)
                throw new InvalidOperationException(
                    "Only draft templates can be modified.");

            var latestVersion = template.Versions
                .OrderByDescending(x => x.VersionNumber)
                .FirstOrDefault();

            if (latestVersion is null)
                throw new InvalidOperationException(
                    "The template does not contain a version.");

            var existingRoles =
                await _recipientRoleRepository.FindAsync(
                    x => x.TemplateVersionId == latestVersion.Id,
                    cancellationToken);

            if (existingRoles.Any(
                x => x.RoutingOrder == request.RoutingOrder))
            {
                throw new InvalidOperationException(
                    "Another recipient already uses this routing order.");
            }

            if (request.DefaultUserId.HasValue)
            {
                var defaultUser =
                    await _userRepository.GetByIdAsync(
                        request.DefaultUserId.Value,
                        cancellationToken);

                if (defaultUser is null ||
                    defaultUser.OrganizationId !=
                        _currentUser.OrganizationId)
                {
                    throw new ArgumentException(
                        "The selected default user is invalid.");
                }
            }

            var role = new TemplateRecipientRole
            {
                Id = Guid.NewGuid(),

                TemplateVersionId = latestVersion.Id,

                Name = request.Name.Trim(),

                RecipientType = request.RecipientType,

                RoutingOrder = request.RoutingOrder,

                IsRequired = request.IsRequired,

                DefaultUserId = request.DefaultUserId,

                AllowSenderEditRecipient =
                    request.AllowSenderEditRecipient,

                AllowSenderDeleteRecipient =
                    request.AllowSenderDeleteRecipient,

                AllowSenderChangeRoutingOrder =
                    request.AllowSenderChangeRoutingOrder
            };

            await _recipientRoleRepository.AddAsync(
                role,
                cancellationToken);

            await _unitOfWork.SaveChangesAsync(
                cancellationToken);

            return new TemplateRecipientRoleDto
            {
                Id = role.Id,
                TemplateVersionId = role.TemplateVersionId,
                Name = role.Name,
                RecipientType = role.RecipientType,
                RoutingOrder = role.RoutingOrder,
                IsRequired = role.IsRequired,
                DefaultUserId = role.DefaultUserId,
                AllowSenderEditRecipient =
                    role.AllowSenderEditRecipient,
                AllowSenderDeleteRecipient =
                    role.AllowSenderDeleteRecipient,
                AllowSenderChangeRoutingOrder =
                    role.AllowSenderChangeRoutingOrder
            };
        }

        public async Task<TemplateDocumentDto> UploadDocumentAsync(Guid templateId, UploadTemplateDocumentRequest request, CancellationToken cancellationToken = default)
        {
            EnsureAuthenticated();

            if (request.Stream == Stream.Null || !request.Stream.CanRead)
                throw new ArgumentException("A valid document stream is required.");

            if (request.FileSize <= 0) throw new ArgumentException("The document is empty");

            if (!string.Equals(
                Path.GetExtension(request.FileName), ".pdf", StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException("Only PDF documents are currently supported");
            }
            var template = await _templateRepository.GetByIdForOrganizationAsync(templateId, _currentUser.OrganizationId, cancellationToken);
            if (template == null) throw new KeyNotFoundException("Template was not found");

            var latestVersion = template.Versions.OrderByDescending(x => x.VersionNumber).FirstOrDefault();
            if (latestVersion == null) throw new InvalidOperationException("The template does not contain a version.");

            var existingDocuments = await _templateDocumentRepository.FindAsync(x => x.TemplateVersionId == latestVersion.Id, cancellationToken);
            var nextOrder = existingDocuments.Count == 0 ? 1 : existingDocuments.Max(x => x.Order) + 1;

            var folder = $"templates/{template.Id}/{latestVersion.Id}/documents";

            string? blobPath = null;

            try
            {
                blobPath = await _fileStorage.UploadAsync(
                    request.Stream,
                    request.FileName,
                    "application/pdf",
                    folder,
                    cancellationToken);

                var document = new TemplateDocument
                {
                    Id = Guid.NewGuid(),

                    TemplateVersionId = latestVersion.Id,

                    OriginalFileName = request.FileName,

                    BlobPath = blobPath,

                    ContentType = "application/pdf",

                    FileSize = request.FileSize,

                    FileHash = null,

                    PageCount = null,

                    Order = nextOrder
                };

                await _templateDocumentRepository.AddAsync(
                    document,
                    cancellationToken);

                await _unitOfWork.SaveChangesAsync(
                    cancellationToken);

                return new TemplateDocumentDto
                {
                    Id = document.Id,
                    TemplateVersionId = document.TemplateVersionId,
                    OriginalFileName = document.OriginalFileName,
                    ContentType = document.ContentType,
                    FileSize = document.FileSize,
                    Order = document.Order,
                    PageCount = document.PageCount
                };
            }
            catch
            {
                if (!string.IsNullOrWhiteSpace(blobPath))
                {
                    await _fileStorage.DeleteAsync(
                        blobPath,
                        cancellationToken);
                }

                throw;

            }
        }

        public async Task<IReadOnlyList<TemplateFieldDto>> GetFieldsAsync(Guid templateId, CancellationToken cancellationToken = default)
        {
            EnsureAuthenticated();

            var template = await _templateRepository.GetByIdForOrganizationAsync(templateId,_currentUser.OrganizationId, cancellationToken);
            if (template == null) throw new KeyNotFoundException("Template was not found");

            var latestVersion = template.Versions.OrderByDescending(x => x.VersionNumber).FirstOrDefault();
            if (latestVersion == null) throw new InvalidOperationException("The template does not contain a version");

            var documents = await _templateDocumentRepository.FindAsync(x => x.TemplateVersionId == latestVersion.Id, cancellationToken);
            if (documents.Count == 0) return Array.Empty<TemplateFieldDto>();


            var documentIds = documents
                .Select(x => x.Id)
                .ToHashSet();

            var fields =
                await _templateFieldRepository.FindAsync(
                    x => documentIds.Contains(x.TemplateDocumentId),
                    cancellationToken);

            return fields
                .OrderBy(x => x.PageNumber)
                .ThenBy(x => x.Y)
                .ThenBy(x => x.X)
                .Select(x => new TemplateFieldDto
                {
                    Id = x.Id,
                    TemplateDocumentId = x.TemplateDocumentId,
                    RecipientRoleId = x.RecipientRoleId,
                    FieldType = x.FieldType,
                    PageNumber = x.PageNumber,
                    X = x.X,
                    Y = x.Y,
                    Width = x.Width,
                    Height = x.Height,
                    IsRequired = x.IsRequired
                })
                .ToList();
        }

        public async Task<TemplateFieldDto> AddFieldsAsync(Guid templateId,TemplateFieldRequest templateFieldRequest, CancellationToken cancellationToken = default)
        {
            EnsureAuthenticated();

            if (templateFieldRequest.TemplateDocumentId == Guid.Empty) throw new ArgumentException("Template document is requied");
            if (templateFieldRequest.RecipientRoleId == Guid.Empty) throw new ArgumentException("Recipient role is requied");
            if (templateFieldRequest.PageNumber <=0) throw new ArgumentException("Page number must be greater than zero.");
             
            ValidateNormalizedCoordinates(templateFieldRequest);
            var template =
        await _templateRepository.GetByIdForOrganizationAsync(
            templateId,
            _currentUser.OrganizationId,
            cancellationToken);

            if (template is null)
                throw new KeyNotFoundException(
                    "Template was not found.");

            if (template.Status != TemplateStatus.Draft)
                throw new InvalidOperationException(
                    "Only draft templates can be modified.");

            var latestVersion = template.Versions
                .OrderByDescending(x => x.VersionNumber)
                .FirstOrDefault();

            if (latestVersion is null)
                throw new InvalidOperationException(
                    "The template does not contain a version.");

            var document =
                await _templateDocumentRepository.GetByIdAsync(
                    templateFieldRequest.TemplateDocumentId,
                    cancellationToken);

            if (document is null ||
                document.TemplateVersionId != latestVersion.Id)
            {
                throw new ArgumentException(
                    "The selected document does not belong to this template version.");
            }

            var recipientRole = await _recipientRoleRepository.GetByIdAsync(templateFieldRequest.RecipientRoleId, cancellationToken);
            if (recipientRole is null || recipientRole.TemplateVersionId != latestVersion.Id)
            {
                throw new ArgumentException(
          "The selected recipient role does not belong to this template version.");
            }

            if (document.PageCount.HasValue &&
                templateFieldRequest.PageNumber > document.PageCount.Value)
            {
                throw new ArgumentException(
                    "The selected page does not exist in the document.");
            }

            var field = new TemplateField
            {
                Id = Guid.NewGuid(),

                TemplateDocumentId =
                    document.Id,

                RecipientRoleId =
                    recipientRole.Id,

                FieldType =
                    templateFieldRequest.FieldType,

                PageNumber =
                    templateFieldRequest.PageNumber,

                X = templateFieldRequest.X,

                Y = templateFieldRequest.Y,

                Width = templateFieldRequest.Width,

                Height = templateFieldRequest.Height,

                IsRequired =
                    templateFieldRequest.IsRequired
            };

            await _templateFieldRepository.AddAsync(
                field,
                cancellationToken);

            await _unitOfWork.SaveChangesAsync(
                cancellationToken);

            return new TemplateFieldDto
            {
                Id = field.Id,
                TemplateDocumentId =
                    field.TemplateDocumentId,
                RecipientRoleId =
                    field.RecipientRoleId,
                FieldType =
                    field.FieldType,
                PageNumber =
                    field.PageNumber,
                X = field.X,
                Y = field.Y,
                Width = field.Width,
                Height = field.Height,
                IsRequired =
                    field.IsRequired
            };

        }

        private static void ValidateNormalizedCoordinates(
    TemplateFieldRequest request)
        {
            if (request.X < 0 || request.X > 1)
                throw new ArgumentException(
                    "X must be between 0 and 1.");

            if (request.Y < 0 || request.Y > 1)
                throw new ArgumentException(
                    "Y must be between 0 and 1.");

            if (request.Width <= 0 || request.Width > 1)
                throw new ArgumentException(
                    "Width must be greater than 0 and not exceed 1.");

            if (request.Height <= 0 || request.Height > 1)
                throw new ArgumentException(
                    "Height must be greater than 0 and not exceed 1.");

            if (request.X + request.Width > 1)
                throw new ArgumentException(
                    "The field exceeds the page width.");

            if (request.Y + request.Height > 1)
                throw new ArgumentException(
                    "The field exceeds the page height.");
        }


        public async Task<TemplateFieldDto> UpdateTemplateFieldAsync(Guid templateId, Guid fieldId, TemplateFieldRequest request, CancellationToken cancellationToken)
        {
            EnsureAuthenticated();

            if (fieldId == Guid.Empty)
                throw new ArgumentException("Field id is required.");

            if (request.TemplateDocumentId == Guid.Empty)
                throw new ArgumentException(
                    "Template document is required.");

            if (request.RecipientRoleId == Guid.Empty)
                throw new ArgumentException(
                    "Recipient role is required.");

            if (request.PageNumber <= 0)
                throw new ArgumentException(
                    "Page number must be greater than zero.");

            ValidateNormalizedCoordinates(
                request.X,
                request.Y,
                request.Width,
                request.Height);


            var template = await _templateRepository.GetByIdForOrganizationAsync(templateId, _currentUser.OrganizationId, cancellationToken);
            if (template is null) throw new KeyNotFoundException("Template was not found.");


            var lastVersion = template.Versions.OrderByDescending(x => x.VersionNumber).FirstOrDefault();
            if (lastVersion is null) throw new InvalidOperationException("The template does not contain a version.");

            var field = await _templateFieldRepository.GetByIdAsync(templateId, cancellationToken);
            if (field is null) throw new KeyNotFoundException("Template field was not found.");

            var currentDocument = await _templateDocumentRepository.GetByIdAsync(field.TemplateDocumentId, cancellationToken);
            if (currentDocument is null || currentDocument.TemplateVersionId!=lastVersion.Id) throw new KeyNotFoundException("Template document was not found");

            var targetDocument = await _templateDocumentRepository.GetByIdAsync(field.TemplateDocumentId, cancellationToken);
            if(targetDocument is null || targetDocument.TemplateVersionId != lastVersion.Id)
            {
                throw new ArgumentException(
        "The selected document does not belong to this template version.");
            }
            var recipientRole =
        await _recipientRoleRepository.GetByIdAsync(
            request.RecipientRoleId,
            cancellationToken);

            if (recipientRole is null ||
                recipientRole.TemplateVersionId != lastVersion.Id)
            {
                throw new ArgumentException(
                    "The selected recipient role does not belong to this template version.");
            }

            if (targetDocument.PageCount.HasValue &&
                request.PageNumber > targetDocument.PageCount.Value)
            {
                throw new ArgumentException(
                    "The selected page does not exist in the document.");
            }

            field.TemplateDocumentId =
                request.TemplateDocumentId;

            field.RecipientRoleId =
                request.RecipientRoleId;

            field.FieldType =
                request.FieldType;

            field.PageNumber =
                request.PageNumber;

            field.X = request.X;
            field.Y = request.Y;
            field.Width = request.Width;
            field.Height = request.Height;

            field.IsRequired =
                request.IsRequired;

            _templateFieldRepository.Update(field);

            await _unitOfWork.SaveChangesAsync(
                cancellationToken);

            return new TemplateFieldDto
            {
                Id = field.Id,
                TemplateDocumentId =
                    field.TemplateDocumentId,
                RecipientRoleId =
                    field.RecipientRoleId,
                FieldType =
                    field.FieldType,
                PageNumber =
                    field.PageNumber,
                X = field.X,
                Y = field.Y,
                Width = field.Width,
                Height = field.Height,
                IsRequired =
                    field.IsRequired
            };
        }

        public async Task DeleteTemplateFieldAsync(Guid templateId, Guid fieldId, CancellationToken cancellationToken)
        {
            var template = await _templateRepository.GetByIdForOrganizationAsync(templateId, _currentUser.OrganizationId, cancellationToken);
            if (template is null) throw new KeyNotFoundException("Template was not found.");


            var lastVersion = template.Versions.OrderByDescending(x => x.VersionNumber).FirstOrDefault();
            if (lastVersion is null) throw new InvalidOperationException("The template does not contain a version.");

            var field = await _templateFieldRepository.GetByIdAsync(templateId, cancellationToken);
            if (field is null) throw new KeyNotFoundException("Template field was not found.");

            var currentDocument = await _templateDocumentRepository.GetByIdAsync(field.TemplateDocumentId, cancellationToken);
            if (currentDocument is null || currentDocument.TemplateVersionId != lastVersion.Id) throw new KeyNotFoundException("Template document was not found");

            _templateFieldRepository.Delete(field);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
             
            }

        public async Task<TemplateRecipientRoleDto> UpdateTemplateRecipientRoledAsync(Guid templateId, Guid roleId, TemplateRecipientRoleRequest request, CancellationToken cancellationToken)
        {
            EnsureAuthenticated();

            if (roleId == Guid.Empty)
                throw new ArgumentException(
                    "Recipient role id is required.");

            if (string.IsNullOrWhiteSpace(request.Name))
                throw new ArgumentException(
                    "Recipient role name is required.");

            if (request.RoutingOrder <= 0)
                throw new ArgumentException(
                    "Routing order must be greater than zero.");

            if (!request.AllowSenderEditRecipient &&
                request.DefaultUserId is null)
            {
                throw new ArgumentException(
                    "A locked recipient must have a default user.");
            }

            var template =
                await _templateRepository.GetByIdForOrganizationAsync(
                    templateId,
                    _currentUser.OrganizationId,
                    cancellationToken);

            if (template is null)
                throw new KeyNotFoundException(
                    "Template was not found.");

            if (template.Status != TemplateStatus.Draft)
                throw new InvalidOperationException(
                    "Only draft templates can be modified.");

            var latestVersion = template.Versions
                .OrderByDescending(x => x.VersionNumber)
                .FirstOrDefault();

            if (latestVersion is null)
                throw new InvalidOperationException(
                    "The template does not contain a version.");

            var role =
                await _recipientRoleRepository.GetByIdAsync(
                    roleId,
                    cancellationToken);

            if (role is null ||
                role.TemplateVersionId != latestVersion.Id)
            {
                throw new KeyNotFoundException(
                    "Recipient role was not found.");
            }

            var existingRoles =
                await _recipientRoleRepository.FindAsync(
                    x =>
                        x.TemplateVersionId == latestVersion.Id &&
                        x.Id != roleId,
                    cancellationToken);

            if (existingRoles.Any(x => x.RoutingOrder == request.RoutingOrder))
            {
                throw new InvalidOperationException(
                    "Another recipient already uses this routing order.");
            }

            if (request.DefaultUserId.HasValue)
            {
                var defaultUser =
                    await _userRepository.GetByIdAsync(
                        request.DefaultUserId.Value,
                        cancellationToken);

                if (defaultUser is null ||
                    !defaultUser.IsActive ||
                    defaultUser.OrganizationId !=
                        _currentUser.OrganizationId)
                {
                    throw new ArgumentException(
                        "The selected default user is invalid.");
                }
            }

            role.Name = request.Name.Trim();

            role.RecipientType =request.RecipientType;

            role.RoutingOrder = request.RoutingOrder;

            role.IsRequired =request.IsRequired;

            role.DefaultUserId =request.DefaultUserId;

            role.AllowSenderEditRecipient =request.AllowSenderEditRecipient;

            role.AllowSenderDeleteRecipient =request.AllowSenderDeleteRecipient;

            role.AllowSenderChangeRoutingOrder =request.AllowSenderChangeRoutingOrder;

            _recipientRoleRepository.Update(role);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new TemplateRecipientRoleDto
            {
                Id = role.Id,
                TemplateVersionId =
                    role.TemplateVersionId,
                Name = role.Name,
                RecipientType =
                    role.RecipientType,
                RoutingOrder =
                    role.RoutingOrder,
                IsRequired =
                    role.IsRequired,
                DefaultUserId =
                    role.DefaultUserId,
                AllowSenderEditRecipient =
                    role.AllowSenderEditRecipient,
                AllowSenderDeleteRecipient =
                    role.AllowSenderDeleteRecipient,
                AllowSenderChangeRoutingOrder =
                    role.AllowSenderChangeRoutingOrder
            };
        }

        public async Task DeleteTemplateRecipientRoleAsync(Guid templateId, Guid roleId, CancellationToken cancellationToken)
        {
            EnsureAuthenticated();
            var template = await _templateRepository.GetByIdForOrganizationAsync(templateId, _currentUser.OrganizationId, cancellationToken);
            if (template is null) throw new KeyNotFoundException("Template was not found.");


            var lastVersion = template.Versions.OrderByDescending(x => x.VersionNumber).FirstOrDefault();
            if (lastVersion is null) throw new InvalidOperationException("The template does not contain a version.");
            var recipientRole = await _recipientRoleRepository.GetByIdAsync(roleId, cancellationToken);

            if (recipientRole is null ||
        recipientRole.TemplateVersionId != lastVersion.Id)
            {
                throw new KeyNotFoundException(
                    "Recipient role was not found.");
            }

            var assignedFields =
                await _templateFieldRepository.FindAsync(
                    x => x.RecipientRoleId == roleId,
                    cancellationToken);

            if (assignedFields.Count > 0)
            {
                throw new InvalidOperationException(
                    "This recipient cannot be deleted because one or more fields are assigned to it.");
            }
            _recipientRoleRepository.Delete(recipientRole);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

        }

        private static void ValidateNormalizedCoordinates(decimal x,decimal y, decimal width,decimal height)
        {
            if (x < 0 || x > 1)
                throw new ArgumentException( "X must be between 0 and 1.");

            if (y < 0 || y > 1)
                throw new ArgumentException("Y must be between 0 and 1.");

            if (width <= 0 || width > 1)
                throw new ArgumentException("Width must be greater than 0 and not exceed 1.");

            if (height <= 0 || height > 1)
                throw new ArgumentException("Height must be greater than 0 and not exceed 1.");

            if (x + width > 1)
                throw new ArgumentException("The field exceeds the page width.");

            if (y + height > 1)
                throw new ArgumentException("The field exceeds the page height.");
        }
    }
}
