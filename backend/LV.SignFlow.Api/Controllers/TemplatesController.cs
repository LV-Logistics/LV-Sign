using LV.SignFlow.Application.Templates;
using LV.SignFlow.Application.Templates.Dtos;
using LV.SignFlow.Application.Templates.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LV.SignFlow.Api.Controllers
{
    [Route("api/templates")]
    [ApiController]
    public class TemplatesController : ControllerBase
    {
        private readonly ITemplateService templateService;
        public TemplatesController(ITemplateService templateService)
        {
            this.templateService = templateService;
        }
        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<TemplateListItemDto>), 200)]
        public async Task<ActionResult<IReadOnlyList<TemplateListItemDto>>> GetAll(CancellationToken cancellationToken)
        {
            var templates=await templateService.GetAllAsync(cancellationToken);
            return Ok(templates);
        }

        [HttpGet("{Id:guid}")]
        [ProducesResponseType(typeof(TemplateDetailsDto), 200)]
        [ProducesResponseType(400)]

        public async Task<ActionResult<TemplateDetailsDto>> GetDetail(Guid Id, CancellationToken cancellationToken)
        {
            var templateDetail = await templateService.GetByIdAsync(Id, cancellationToken);
            if(templateDetail == null)  return NotFound();
            return Ok(templateDetail);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Guid),200)]
        [ProducesResponseType(400)]

        public async Task<ActionResult<Guid>> CreateTemplate(CreateTemplateRequest request,CancellationToken cancellationToken)
        {
           var templateId= await templateService.CreateAsync(request, cancellationToken); 
            return CreatedAtAction(nameof(CreateTemplate), new {id=templateId}, templateId);
        }

        [HttpGet("{templateId:guid}/recipient-roles")]
        [ProducesResponseType(
    typeof(IReadOnlyList<TemplateRecipientRoleDto>),
    StatusCodes.Status200OK)]
        [ProducesResponseType(
    StatusCodes.Status404NotFound)]

        public async Task<ActionResult<IReadOnlyList<TemplateRecipientRoleDto>>> GetRecipientRoles(Guid templateId,CancellationToken cancellationToken)
        {
            try
            {
                var roles =
                    await templateService.GetRecipientRoleAsync(
                        templateId,
                        cancellationToken);

                return Ok(roles);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost("{templateId:guid}/recipient-roles")]
        [ProducesResponseType(
     typeof(TemplateRecipientRoleDto),
     StatusCodes.Status201Created)]
        [ProducesResponseType(
     StatusCodes.Status400BadRequest)]
        [ProducesResponseType(
     StatusCodes.Status404NotFound)]
        [ProducesResponseType(
     StatusCodes.Status409Conflict)]

        public async Task<ActionResult<Guid>> AddRecipientRole(Guid templateId,TemplateRecipientRoleRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var role =
                    await templateService.AddRecipientRoleAsync(
                        templateId,
                        request,
                        cancellationToken);

                return StatusCode(
                    StatusCodes.Status201Created,
                    role);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }


        [HttpPost("{templateId:guid}/documents")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(
    typeof(TemplateDocumentDto),
    StatusCodes.Status201Created)]
        [ProducesResponseType(
    StatusCodes.Status400BadRequest)]
        [ProducesResponseType(
    StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TemplateDocumentDto>>
    UploadDocument(
        Guid templateId,
        IFormFile file,
        CancellationToken cancellationToken)
        {
            if (file.Length == 0)
                return BadRequest("The uploaded file is empty.");

            await using var stream = file.OpenReadStream();

            var request = new UploadTemplateDocumentRequest
            {
                Stream = stream,
                FileName = file.FileName,
                ContentType = file.ContentType,
                FileSize = file.Length
            };

            try
            {
                var document =
                    await templateService.UploadDocumentAsync(templateId,request,cancellationToken);

                return StatusCode(
                    StatusCodes.Status201Created,
                    document);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{templateId:guid}/fields")]
        [ProducesResponseType(
     typeof(IReadOnlyList<TemplateFieldDto>),
     StatusCodes.Status200OK)]
        [ProducesResponseType(
     StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IReadOnlyList<TemplateFieldDto>>> GetTemplateFields(Guid templateId,CancellationToken cancellationToken)
        {
            try
            {
                var fields = await templateService.GetFieldsAsync(templateId, cancellationToken);
                return Ok(fields);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost("{templateId:guid}/fields")]
        [ProducesResponseType(
       typeof(TemplateFieldDto),
       StatusCodes.Status201Created)]
        [ProducesResponseType(
       StatusCodes.Status400BadRequest)]
        [ProducesResponseType(
       StatusCodes.Status404NotFound)]
        [ProducesResponseType(
       StatusCodes.Status409Conflict)]
        public async Task<ActionResult<TemplateFieldDto>> AddTemplateField(Guid templateId,TemplateFieldRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var field = await templateService.AddFieldsAsync(templateId,request, cancellationToken);

                return StatusCode(StatusCodes.Status201Created, field);

            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpPut("{templateId:guid}/fields/{fieldId:guid}")]
        [ProducesResponseType(
        typeof(TemplateFieldDto),
        StatusCodes.Status200OK)]
        [ProducesResponseType(
        StatusCodes.Status400BadRequest)]
        [ProducesResponseType(
        StatusCodes.Status404NotFound)]
        [ProducesResponseType(
        StatusCodes.Status409Conflict)]
        public async Task<ActionResult<TemplateFieldDto>> UpdateTemplateField(Guid templateId,Guid fieldId,TemplateFieldRequest request,CancellationToken cancellationToken)
        {
            try
            {
                var field = await templateService.UpdateTemplateFieldAsync(templateId, fieldId, request, cancellationToken);
                return Ok(field);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpDelete("{templateId:guid}/fields/{fieldId:guid}")]
        [ProducesResponseType(
    StatusCodes.Status204NoContent)]
        [ProducesResponseType(
    StatusCodes.Status404NotFound)]
        [ProducesResponseType(
    StatusCodes.Status409Conflict)]
        public async Task<IActionResult> DeleteField(
    Guid templateId,
    Guid fieldId,
    CancellationToken cancellationToken)
        {
            try
            {
                await templateService.DeleteTemplateFieldAsync(
                    templateId,
                    fieldId,
                    cancellationToken);

                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }


        [HttpPut(
    "{templateId:guid}/recipient-roles/{roleId:guid}")]
        [ProducesResponseType(
    typeof(TemplateRecipientRoleDto),
    StatusCodes.Status200OK)]
        [ProducesResponseType(
    StatusCodes.Status400BadRequest)]
        [ProducesResponseType(
    StatusCodes.Status404NotFound)]
        [ProducesResponseType(
    StatusCodes.Status409Conflict)]
        public async Task<ActionResult<TemplateRecipientRoleDto>>
    UpdateRecipientRole(
        Guid templateId,
        Guid roleId,
        TemplateRecipientRoleRequest request,
        CancellationToken cancellationToken)
        {
            try
            {
                var role =
                    await templateService.UpdateTemplateRecipientRoledAsync(
                        templateId,
                        roleId,
                        request,
                        cancellationToken);

                return Ok(role);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpDelete(
    "{templateId:guid}/recipient-roles/{roleId:guid}")]
        [ProducesResponseType(
    StatusCodes.Status204NoContent)]
        [ProducesResponseType(
    StatusCodes.Status404NotFound)]
        [ProducesResponseType(
    StatusCodes.Status409Conflict)]
        public async Task<IActionResult>
    DeleteRecipientRole(
        Guid templateId,
        Guid roleId,
        CancellationToken cancellationToken)
        {
            try
            {
                await templateService.DeleteTemplateRecipientRoleAsync(
                    templateId,
                    roleId,
                    cancellationToken);

                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

    }
}
