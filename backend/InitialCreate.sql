IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
CREATE TABLE [Organizations] (
    [Id] uniqueidentifier NOT NULL,
    [Name] nvarchar(200) NOT NULL,
    [Code] nvarchar(50) NULL,
    [IsActive] bit NOT NULL,
    [CreatedAt] datetimeoffset NOT NULL,
    [UpdatedAt] datetimeoffset NOT NULL,
    CONSTRAINT [PK_Organizations] PRIMARY KEY ([Id])
);

CREATE TABLE [PermissionProfiles] (
    [Id] uniqueidentifier NOT NULL,
    [Name] nvarchar(150) NOT NULL,
    [Description] nvarchar(500) NULL,
    [IsSystemProfile] bit NOT NULL,
    [IsActive] bit NOT NULL,
    [CreatedAt] datetimeoffset NOT NULL,
    [UpdatedAt] datetimeoffset NOT NULL,
    CONSTRAINT [PK_PermissionProfiles] PRIMARY KEY ([Id])
);

CREATE TABLE [Permissions] (
    [Id] uniqueidentifier NOT NULL,
    [Code] nvarchar(150) NOT NULL,
    [Name] nvarchar(150) NOT NULL,
    [Description] nvarchar(500) NULL,
    [Category] nvarchar(100) NULL,
    CONSTRAINT [PK_Permissions] PRIMARY KEY ([Id])
);

CREATE TABLE [Roles] (
    [Id] uniqueidentifier NOT NULL,
    [Name] nvarchar(100) NOT NULL,
    [Description] nvarchar(500) NULL,
    [IsSystemRole] bit NOT NULL,
    [IsActive] bit NOT NULL,
    [CreatedAt] datetimeoffset NOT NULL,
    [UpdatedAt] datetimeoffset NOT NULL,
    CONSTRAINT [PK_Roles] PRIMARY KEY ([Id])
);

CREATE TABLE [Departments] (
    [Id] uniqueidentifier NOT NULL,
    [OrganizationId] uniqueidentifier NOT NULL,
    [Name] nvarchar(200) NOT NULL,
    [Code] nvarchar(50) NULL,
    [IsActive] bit NOT NULL,
    [CreatedAt] datetimeoffset NOT NULL,
    [UpdatedAt] datetimeoffset NOT NULL,
    CONSTRAINT [PK_Departments] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Departments_Organizations_OrganizationId] FOREIGN KEY ([OrganizationId]) REFERENCES [Organizations] ([Id]) ON DELETE SET NULL
);

CREATE TABLE [PermissionProfilePermissions] (
    [PermissionProfileId] uniqueidentifier NOT NULL,
    [PermissionId] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_PermissionProfilePermissions] PRIMARY KEY ([PermissionProfileId], [PermissionId]),
    CONSTRAINT [FK_PermissionProfilePermissions_PermissionProfiles_PermissionProfileId] FOREIGN KEY ([PermissionProfileId]) REFERENCES [PermissionProfiles] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_PermissionProfilePermissions_Permissions_PermissionId] FOREIGN KEY ([PermissionId]) REFERENCES [Permissions] ([Id]) ON DELETE NO ACTION
);

CREATE TABLE [RolePermissions] (
    [RoleId] uniqueidentifier NOT NULL,
    [PermissionId] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_RolePermissions] PRIMARY KEY ([RoleId], [PermissionId]),
    CONSTRAINT [FK_RolePermissions_Permissions_PermissionId] FOREIGN KEY ([PermissionId]) REFERENCES [Permissions] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_RolePermissions_Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [Roles] ([Id]) ON DELETE NO ACTION
);

CREATE TABLE [Users] (
    [Id] uniqueidentifier NOT NULL,
    [Email] nvarchar(320) NOT NULL,
    [DisplayName] nvarchar(200) NOT NULL,
    [JobTitle] nvarchar(320) NULL,
    [OrganizationId] uniqueidentifier NOT NULL,
    [DepartmentId] uniqueidentifier NULL,
    [ExternalIdentityId] nvarchar(320) NULL,
    [IsActive] bit NOT NULL,
    [LastLoginAt] datetimeoffset NULL,
    [CreatedAt] datetimeoffset NOT NULL,
    [UpdatedAt] datetimeoffset NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Users_Departments_DepartmentId] FOREIGN KEY ([DepartmentId]) REFERENCES [Departments] ([Id]) ON DELETE SET NULL,
    CONSTRAINT [FK_Users_Organizations_OrganizationId] FOREIGN KEY ([OrganizationId]) REFERENCES [Organizations] ([Id]) ON DELETE NO ACTION
);

CREATE TABLE [AuditLogs] (
    [Id] uniqueidentifier NOT NULL,
    [UserId] uniqueidentifier NULL,
    [Action] nvarchar(200) NOT NULL,
    [EntityType] nvarchar(200) NULL,
    [EntityId] uniqueidentifier NULL,
    [DetailsJson] nvarchar(max) NULL,
    [IpAddress] nvarchar(45) NULL,
    [UserAgent] nvarchar(1000) NULL,
    [OccurredAt] datetimeoffset NOT NULL,
    CONSTRAINT [PK_AuditLogs] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AuditLogs_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE SET NULL
);

CREATE TABLE [Signatures] (
    [Id] uniqueidentifier NOT NULL,
    [UserId] uniqueidentifier NOT NULL,
    [Type] int NOT NULL,
    [BlobPath] nvarchar(1000) NOT NULL,
    [IsDefault] bit NOT NULL,
    [IsActive] bit NOT NULL,
    [CreatedAt] datetimeoffset NOT NULL,
    [UpdatedAt] datetimeoffset NOT NULL,
    CONSTRAINT [PK_Signatures] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Signatures_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION
);

CREATE TABLE [Stamps] (
    [Id] uniqueidentifier NOT NULL,
    [Name] nvarchar(200) NOT NULL,
    [BlobPath] nvarchar(1000) NOT NULL,
    [IsActive] bit NOT NULL,
    [CreatedByUserId] uniqueidentifier NOT NULL,
    [CreatedAt] datetimeoffset NOT NULL,
    [UpdatedAt] datetimeoffset NOT NULL,
    CONSTRAINT [PK_Stamps] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Stamps_Users_CreatedByUserId] FOREIGN KEY ([CreatedByUserId]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION
);

CREATE TABLE [Templates] (
    [Id] uniqueidentifier NOT NULL,
    [Name] nvarchar(200) NOT NULL,
    [Description] nvarchar(500) NULL,
    [OwnerUserId] uniqueidentifier NOT NULL,
    [Status] int NOT NULL,
    [IsDeleted] bit NOT NULL,
    [DeletedAt] datetimeoffset NULL,
    [CreatedAt] datetimeoffset NULL,
    [UpdatedAt] datetimeoffset NULL,
    [OrganizationId] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_Templates] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Templates_Organizations_OrganizationId] FOREIGN KEY ([OrganizationId]) REFERENCES [Organizations] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Templates_Users_OwnerUserId] FOREIGN KEY ([OwnerUserId]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION
);

CREATE TABLE [UserPermissionProfiles] (
    [UserId] uniqueidentifier NOT NULL,
    [PermissionProfileId] uniqueidentifier NOT NULL,
    [AssignedAt] datetimeoffset NOT NULL,
    [AssignedByUserId] uniqueidentifier NULL,
    CONSTRAINT [PK_UserPermissionProfiles] PRIMARY KEY ([UserId], [PermissionProfileId]),
    CONSTRAINT [FK_UserPermissionProfiles_PermissionProfiles_PermissionProfileId] FOREIGN KEY ([PermissionProfileId]) REFERENCES [PermissionProfiles] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_UserPermissionProfiles_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION
);

CREATE TABLE [UserRoles] (
    [UserId] uniqueidentifier NOT NULL,
    [RoleId] uniqueidentifier NOT NULL,
    [AssignedAt] datetimeoffset NOT NULL,
    [AssignedByUserId] uniqueidentifier NULL,
    CONSTRAINT [PK_UserRoles] PRIMARY KEY ([UserId], [RoleId]),
    CONSTRAINT [FK_UserRoles_Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [Roles] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_UserRoles_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION
);

CREATE TABLE [TemplateShares] (
    [Id] uniqueidentifier NOT NULL,
    [TemplateId] uniqueidentifier NOT NULL,
    [SharedWithUserId] uniqueidentifier NULL,
    [SharedWithDepartmentId] uniqueidentifier NULL,
    [AccessLevel] int NOT NULL,
    [SharedByUserId] uniqueidentifier NOT NULL,
    [SharedAt] datetimeoffset NOT NULL,
    CONSTRAINT [PK_TemplateShares] PRIMARY KEY ([Id]),
    CONSTRAINT [CK_TemplateShares_ExactlyOneTarget] CHECK ((
                    ([SharedWithUserId] IS NOT NULL AND [SharedWithDepartmentId] IS NULL)
                    OR
                    ([SharedWithUserId] IS NULL AND [SharedWithDepartmentId] IS NOT NULL)
                  )),
    CONSTRAINT [FK_TemplateShares_Departments_SharedWithDepartmentId] FOREIGN KEY ([SharedWithDepartmentId]) REFERENCES [Departments] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_TemplateShares_Templates_TemplateId] FOREIGN KEY ([TemplateId]) REFERENCES [Templates] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_TemplateShares_Users_SharedByUserId] FOREIGN KEY ([SharedByUserId]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_TemplateShares_Users_SharedWithUserId] FOREIGN KEY ([SharedWithUserId]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION
);

CREATE TABLE [TemplateVersions] (
    [Id] uniqueidentifier NOT NULL,
    [TemplateId] uniqueidentifier NOT NULL,
    [VersionNumber] int NOT NULL,
    [IsPublished] bit NOT NULL,
    [CreatedAt] datetimeoffset NOT NULL,
    [CreatedByUserId] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_TemplateVersions] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_TemplateVersions_Templates_TemplateId] FOREIGN KEY ([TemplateId]) REFERENCES [Templates] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_TemplateVersions_Users_CreatedByUserId] FOREIGN KEY ([CreatedByUserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [Envelopes] (
    [Id] uniqueidentifier NOT NULL,
    [Subject] nvarchar(500) NOT NULL,
    [Message] nvarchar(4000) NULL,
    [Type] int NOT NULL,
    [Status] int NOT NULL,
    [CreatedByUserId] uniqueidentifier NOT NULL,
    [SourceTemplateVersionId] uniqueidentifier NULL,
    [CreatedAt] datetimeoffset NOT NULL,
    [CompletedAt] datetimeoffset NULL,
    [SentAt] datetimeoffset NULL,
    [ExpiresAt] datetimeoffset NULL,
    [VoidedAt] datetimeoffset NULL,
    [DeletedAt] datetimeoffset NULL,
    [UpdatedAt] datetimeoffset NOT NULL,
    [IsDeleted] bit NOT NULL,
    [OrganizationId] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_Envelopes] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Envelopes_Organizations_OrganizationId] FOREIGN KEY ([OrganizationId]) REFERENCES [Organizations] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Envelopes_TemplateVersions_SourceTemplateVersionId] FOREIGN KEY ([SourceTemplateVersionId]) REFERENCES [TemplateVersions] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Envelopes_Users_CreatedByUserId] FOREIGN KEY ([CreatedByUserId]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION
);

CREATE TABLE [TemplateDocuments] (
    [Id] uniqueidentifier NOT NULL,
    [TemplateVersionId] uniqueidentifier NOT NULL,
    [OriginalFileName] nvarchar(500) NOT NULL,
    [BlobPath] nvarchar(1000) NOT NULL,
    [ContentType] nvarchar(200) NOT NULL,
    [PageCount] int NULL,
    [FileSize] bigint NOT NULL,
    [FileHash] nvarchar(128) NULL,
    [Order] int NOT NULL,
    CONSTRAINT [PK_TemplateDocuments] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_TemplateDocuments_TemplateVersions_TemplateVersionId] FOREIGN KEY ([TemplateVersionId]) REFERENCES [TemplateVersions] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [TemplateRecipientRoles] (
    [Id] uniqueidentifier NOT NULL,
    [TemplateVersionId] uniqueidentifier NOT NULL,
    [Name] nvarchar(200) NOT NULL,
    [RecipientType] int NOT NULL,
    [RoutingOrder] int NOT NULL,
    [IsRequired] bit NOT NULL,
    [DefaultUserId] uniqueidentifier NULL,
    [AllowSenderEditRecipient] bit NOT NULL,
    [AllowSenderDeleteRecipient] bit NOT NULL,
    [AllowSenderChangeRoutingOrder] bit NOT NULL,
    CONSTRAINT [PK_TemplateRecipientRoles] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_TemplateRecipientRoles_TemplateVersions_TemplateVersionId] FOREIGN KEY ([TemplateVersionId]) REFERENCES [TemplateVersions] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_TemplateRecipientRoles_Users_DefaultUserId] FOREIGN KEY ([DefaultUserId]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION
);

CREATE TABLE [TemplateRoutingRuleSets] (
    [Id] uniqueidentifier NOT NULL,
    [TemplateVersionId] uniqueidentifier NOT NULL,
    [Name] nvarchar(200) NOT NULL,
    [IsActive] bit NOT NULL,
    [CreatedAt] datetimeoffset NOT NULL,
    [UpdatedAt] datetimeoffset NOT NULL,
    CONSTRAINT [PK_TemplateRoutingRuleSets] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_TemplateRoutingRuleSets_TemplateVersions_TemplateVersionId] FOREIGN KEY ([TemplateVersionId]) REFERENCES [TemplateVersions] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [EnvelopeDocuments] (
    [Id] uniqueidentifier NOT NULL,
    [EnvelopeId] uniqueidentifier NOT NULL,
    [OriginalFileName] nvarchar(500) NOT NULL,
    [OriginalBlobPath] nvarchar(1000) NOT NULL,
    [FinalBlobPath] nvarchar(1000) NULL,
    [ContentType] nvarchar(200) NOT NULL,
    [FileSize] bigint NOT NULL,
    [OriginalHash] nvarchar(128) NULL,
    [FinalHash] nvarchar(128) NULL,
    [Order] int NOT NULL,
    [PageCount] int NULL,
    [CreatedAt] datetimeoffset NOT NULL,
    CONSTRAINT [PK_EnvelopeDocuments] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_EnvelopeDocuments_Envelopes_EnvelopeId] FOREIGN KEY ([EnvelopeId]) REFERENCES [Envelopes] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [EnvelopeRecipients] (
    [Id] uniqueidentifier NOT NULL,
    [EnvelopeId] uniqueidentifier NOT NULL,
    [UserId] uniqueidentifier NULL,
    [Name] nvarchar(200) NOT NULL,
    [Email] nvarchar(320) NOT NULL,
    [RecipientType] int NOT NULL,
    [Status] int NOT NULL,
    [RoutingOrder] int NOT NULL,
    [SentAt] datetimeoffset NULL,
    [ViewedAt] datetimeoffset NULL,
    [CompletedAt] datetimeoffset NULL,
    [DeclinedAt] datetimeoffset NULL,
    [DeclineReason] nvarchar(1000) NULL,
    [AllowSenderEditRecipient] bit NOT NULL,
    [AllowSenderDeleteRecipient] bit NOT NULL,
    [AllowSenderChangeRoutingOrder] bit NOT NULL,
    CONSTRAINT [PK_EnvelopeRecipients] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_EnvelopeRecipients_Envelopes_EnvelopeId] FOREIGN KEY ([EnvelopeId]) REFERENCES [Envelopes] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_EnvelopeRecipients_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE SET NULL
);

CREATE TABLE [EnvelopeRoutingRuleSets] (
    [Id] uniqueidentifier NOT NULL,
    [EnvelopeId] uniqueidentifier NOT NULL,
    [Name] nvarchar(200) NOT NULL,
    [IsActive] bit NOT NULL,
    CONSTRAINT [PK_EnvelopeRoutingRuleSets] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_EnvelopeRoutingRuleSets_Envelopes_EnvelopeId] FOREIGN KEY ([EnvelopeId]) REFERENCES [Envelopes] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [EnvelopeStatusHistories] (
    [Id] uniqueidentifier NOT NULL,
    [EnvelopeId] uniqueidentifier NOT NULL,
    [PreviousStatus] int NULL,
    [NewStatus] int NOT NULL,
    [ChangedByUserId] uniqueidentifier NULL,
    [Reason] nvarchar(1000) NULL,
    [ChangedAt] datetimeoffset NOT NULL,
    CONSTRAINT [PK_EnvelopeStatusHistories] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_EnvelopeStatusHistories_Envelopes_EnvelopeId] FOREIGN KEY ([EnvelopeId]) REFERENCES [Envelopes] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_EnvelopeStatusHistories_Users_ChangedByUserId] FOREIGN KEY ([ChangedByUserId]) REFERENCES [Users] ([Id]) ON DELETE SET NULL
);

CREATE TABLE [TemplateFields] (
    [Id] uniqueidentifier NOT NULL,
    [TemplateDocumentId] uniqueidentifier NOT NULL,
    [RecipientRoleId] uniqueidentifier NOT NULL,
    [FieldType] int NOT NULL,
    [PageNumber] int NOT NULL,
    [X] decimal(9,6) NOT NULL,
    [Y] decimal(9,6) NOT NULL,
    [Width] decimal(9,6) NOT NULL,
    [Height] decimal(9,6) NOT NULL,
    [IsRequired] bit NOT NULL,
    CONSTRAINT [PK_TemplateFields] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_TemplateFields_TemplateDocuments_TemplateDocumentId] FOREIGN KEY ([TemplateDocumentId]) REFERENCES [TemplateDocuments] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_TemplateFields_TemplateRecipientRoles_RecipientRoleId] FOREIGN KEY ([RecipientRoleId]) REFERENCES [TemplateRecipientRoles] ([Id]) ON DELETE NO ACTION
);

CREATE TABLE [TemplateRoutingRules] (
    [Id] uniqueidentifier NOT NULL,
    [TemplateRoutingRuleSetId] uniqueidentifier NOT NULL,
    [TargetRecipientRoleId] uniqueidentifier NOT NULL,
    [Name] nvarchar(200) NULL,
    [MatchType] int NOT NULL,
    [Priority] int NOT NULL,
    [IsActive] bit NOT NULL,
    CONSTRAINT [PK_TemplateRoutingRules] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_TemplateRoutingRules_TemplateRecipientRoles_TargetRecipientRoleId] FOREIGN KEY ([TargetRecipientRoleId]) REFERENCES [TemplateRecipientRoles] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_TemplateRoutingRules_TemplateRoutingRuleSets_TemplateRoutingRuleSetId] FOREIGN KEY ([TemplateRoutingRuleSetId]) REFERENCES [TemplateRoutingRuleSets] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [EnvelopeFields] (
    [Id] uniqueidentifier NOT NULL,
    [EnvelopeId] uniqueidentifier NOT NULL,
    [EnvelopeDocumentId] uniqueidentifier NOT NULL,
    [EnvelopeRecipientId] uniqueidentifier NOT NULL,
    [FieldType] int NOT NULL,
    [PageNumber] int NOT NULL,
    [X] decimal(9,6) NOT NULL,
    [Y] decimal(9,6) NOT NULL,
    [Width] decimal(9,6) NOT NULL,
    [Height] decimal(9,6) NOT NULL,
    [IsRequired] bit NOT NULL,
    [Value] nvarchar(4000) NULL,
    [CompletedAt] datetimeoffset NULL,
    CONSTRAINT [PK_EnvelopeFields] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_EnvelopeFields_EnvelopeDocuments_EnvelopeDocumentId] FOREIGN KEY ([EnvelopeDocumentId]) REFERENCES [EnvelopeDocuments] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_EnvelopeFields_EnvelopeRecipients_EnvelopeRecipientId] FOREIGN KEY ([EnvelopeRecipientId]) REFERENCES [EnvelopeRecipients] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_EnvelopeFields_Envelopes_EnvelopeId] FOREIGN KEY ([EnvelopeId]) REFERENCES [Envelopes] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [EvelopeEvents] (
    [Id] uniqueidentifier NOT NULL,
    [EnvelopeId] uniqueidentifier NOT NULL,
    [EventType] int NOT NULL,
    [UserId] uniqueidentifier NULL,
    [RecipientId] uniqueidentifier NULL,
    [OccurredAt] datetimeoffset NOT NULL,
    [IpAddress] nvarchar(45) NULL,
    [UserAgent] nvarchar(1000) NULL,
    [MetadataJson] nvarchar(max) NULL,
    CONSTRAINT [PK_EvelopeEvents] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_EvelopeEvents_EnvelopeRecipients_RecipientId] FOREIGN KEY ([RecipientId]) REFERENCES [EnvelopeRecipients] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_EvelopeEvents_Envelopes_EnvelopeId] FOREIGN KEY ([EnvelopeId]) REFERENCES [Envelopes] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_EvelopeEvents_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE SET NULL
);

CREATE TABLE [EnvelopeRoutingRules] (
    [Id] uniqueidentifier NOT NULL,
    [EnvelopeRoutingRuleSetId] uniqueidentifier NOT NULL,
    [TargetRecipientId] uniqueidentifier NOT NULL,
    [Name] nvarchar(200) NULL,
    [MatchType] int NOT NULL,
    [Priority] int NOT NULL,
    [IsActive] bit NOT NULL,
    CONSTRAINT [PK_EnvelopeRoutingRules] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_EnvelopeRoutingRules_EnvelopeRecipients_TargetRecipientId] FOREIGN KEY ([TargetRecipientId]) REFERENCES [EnvelopeRecipients] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_EnvelopeRoutingRules_EnvelopeRoutingRuleSets_EnvelopeRoutingRuleSetId] FOREIGN KEY ([EnvelopeRoutingRuleSetId]) REFERENCES [EnvelopeRoutingRuleSets] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [TemplateRoutingConditions] (
    [Id] uniqueidentifier NOT NULL,
    [TemplateRoutingRuleId] uniqueidentifier NOT NULL,
    [SourceTemplateFieldId] uniqueidentifier NOT NULL,
    [Operator] int NOT NULL,
    [ComparisonValue] nvarchar(1000) NULL,
    CONSTRAINT [PK_TemplateRoutingConditions] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_TemplateRoutingConditions_TemplateFields_SourceTemplateFieldId] FOREIGN KEY ([SourceTemplateFieldId]) REFERENCES [TemplateFields] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_TemplateRoutingConditions_TemplateRoutingRules_TemplateRoutingRuleId] FOREIGN KEY ([TemplateRoutingRuleId]) REFERENCES [TemplateRoutingRules] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [EnvelopeRoutingConditions] (
    [Id] uniqueidentifier NOT NULL,
    [EnvelopeRoutingRuleId] uniqueidentifier NOT NULL,
    [SourceEnvelopeFieldId] uniqueidentifier NOT NULL,
    [Operator] int NOT NULL,
    [ComparisonValue] nvarchar(1000) NULL,
    CONSTRAINT [PK_EnvelopeRoutingConditions] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_EnvelopeRoutingConditions_EnvelopeFields_SourceEnvelopeFieldId] FOREIGN KEY ([SourceEnvelopeFieldId]) REFERENCES [EnvelopeFields] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_EnvelopeRoutingConditions_EnvelopeRoutingRules_EnvelopeRoutingRuleId] FOREIGN KEY ([EnvelopeRoutingRuleId]) REFERENCES [EnvelopeRoutingRules] ([Id]) ON DELETE CASCADE
);

CREATE INDEX [IX_AuditLogs_Action] ON [AuditLogs] ([Action]);

CREATE INDEX [IX_AuditLogs_EntityType_EntityId] ON [AuditLogs] ([EntityType], [EntityId]);

CREATE INDEX [IX_AuditLogs_OccurredAt] ON [AuditLogs] ([OccurredAt]);

CREATE INDEX [IX_AuditLogs_UserId] ON [AuditLogs] ([UserId]);

CREATE UNIQUE INDEX [IX_Departments_OrganizationId_Name] ON [Departments] ([OrganizationId], [Name]);

CREATE INDEX [IX_EnvelopeDocuments_EnvelopeId_Order] ON [EnvelopeDocuments] ([EnvelopeId], [Order]);

CREATE INDEX [IX_EnvelopeFields_EnvelopeDocumentId] ON [EnvelopeFields] ([EnvelopeDocumentId]);

CREATE INDEX [IX_EnvelopeFields_EnvelopeId] ON [EnvelopeFields] ([EnvelopeId]);

CREATE INDEX [IX_EnvelopeFields_EnvelopeRecipientId] ON [EnvelopeFields] ([EnvelopeRecipientId]);

CREATE INDEX [IX_EnvelopeRecipients_EnvelopeId] ON [EnvelopeRecipients] ([EnvelopeId]);

CREATE INDEX [IX_EnvelopeRecipients_EnvelopeId_RoutingOrder] ON [EnvelopeRecipients] ([EnvelopeId], [RoutingOrder]);

CREATE INDEX [IX_EnvelopeRecipients_UserId] ON [EnvelopeRecipients] ([UserId]);

CREATE INDEX [IX_EnvelopeRoutingConditions_EnvelopeRoutingRuleId] ON [EnvelopeRoutingConditions] ([EnvelopeRoutingRuleId]);

CREATE INDEX [IX_EnvelopeRoutingConditions_SourceEnvelopeFieldId] ON [EnvelopeRoutingConditions] ([SourceEnvelopeFieldId]);

CREATE UNIQUE INDEX [IX_EnvelopeRoutingRules_EnvelopeRoutingRuleSetId_Priority] ON [EnvelopeRoutingRules] ([EnvelopeRoutingRuleSetId], [Priority]);

CREATE INDEX [IX_EnvelopeRoutingRules_TargetRecipientId] ON [EnvelopeRoutingRules] ([TargetRecipientId]);

CREATE INDEX [IX_EnvelopeRoutingRuleSets_EnvelopeId] ON [EnvelopeRoutingRuleSets] ([EnvelopeId]);

CREATE INDEX [IX_Envelopes_CreatedByUserId] ON [Envelopes] ([CreatedByUserId]);

CREATE INDEX [IX_Envelopes_OrganizationId] ON [Envelopes] ([OrganizationId]);

CREATE INDEX [IX_Envelopes_OrganizationId_Status] ON [Envelopes] ([OrganizationId], [Status]);

CREATE INDEX [IX_Envelopes_SourceTemplateVersionId] ON [Envelopes] ([SourceTemplateVersionId]);

CREATE INDEX [IX_EnvelopeStatusHistories_ChangedByUserId] ON [EnvelopeStatusHistories] ([ChangedByUserId]);

CREATE INDEX [IX_EnvelopeStatusHistories_EnvelopeId_ChangedAt] ON [EnvelopeStatusHistories] ([EnvelopeId], [ChangedAt]);

CREATE INDEX [IX_EvelopeEvents_EnvelopeId_OccurredAt] ON [EvelopeEvents] ([EnvelopeId], [OccurredAt]);

CREATE INDEX [IX_EvelopeEvents_RecipientId] ON [EvelopeEvents] ([RecipientId]);

CREATE INDEX [IX_EvelopeEvents_UserId] ON [EvelopeEvents] ([UserId]);

CREATE UNIQUE INDEX [IX_Organizations_Code] ON [Organizations] ([Code]) WHERE [Code] is not null;

CREATE INDEX [IX_PermissionProfilePermissions_PermissionId] ON [PermissionProfilePermissions] ([PermissionId]);

CREATE UNIQUE INDEX [IX_PermissionProfiles_Name] ON [PermissionProfiles] ([Name]);

CREATE UNIQUE INDEX [IX_Permissions_Code] ON [Permissions] ([Code]);

CREATE INDEX [IX_RolePermissions_PermissionId] ON [RolePermissions] ([PermissionId]);

CREATE UNIQUE INDEX [IX_Roles_Name] ON [Roles] ([Name]);

CREATE UNIQUE INDEX [IX_Signatures_UserId] ON [Signatures] ([UserId]) WHERE [IsDefault] = 1 AND [IsActive] = 1;

CREATE INDEX [IX_Stamps_CreatedByUserId] ON [Stamps] ([CreatedByUserId]);

CREATE INDEX [IX_Stamps_Name] ON [Stamps] ([Name]);

CREATE INDEX [IX_TemplateDocuments_TemplateVersionId_Order] ON [TemplateDocuments] ([TemplateVersionId], [Order]);

CREATE INDEX [IX_TemplateFields_RecipientRoleId] ON [TemplateFields] ([RecipientRoleId]);

CREATE INDEX [IX_TemplateFields_TemplateDocumentId] ON [TemplateFields] ([TemplateDocumentId]);

CREATE INDEX [IX_TemplateRecipientRoles_DefaultUserId] ON [TemplateRecipientRoles] ([DefaultUserId]);

CREATE INDEX [IX_TemplateRecipientRoles_TemplateVersionId_RoutingOrder] ON [TemplateRecipientRoles] ([TemplateVersionId], [RoutingOrder]);

CREATE INDEX [IX_TemplateRoutingConditions_SourceTemplateFieldId] ON [TemplateRoutingConditions] ([SourceTemplateFieldId]);

CREATE INDEX [IX_TemplateRoutingConditions_TemplateRoutingRuleId] ON [TemplateRoutingConditions] ([TemplateRoutingRuleId]);

CREATE INDEX [IX_TemplateRoutingRules_TargetRecipientRoleId] ON [TemplateRoutingRules] ([TargetRecipientRoleId]);

CREATE UNIQUE INDEX [IX_TemplateRoutingRules_TemplateRoutingRuleSetId_Priority] ON [TemplateRoutingRules] ([TemplateRoutingRuleSetId], [Priority]);

CREATE INDEX [IX_TemplateRoutingRuleSets_TemplateVersionId] ON [TemplateRoutingRuleSets] ([TemplateVersionId]);

CREATE UNIQUE INDEX [IX_Templates_Name] ON [Templates] ([Name]);

CREATE INDEX [IX_Templates_OrganizationId] ON [Templates] ([OrganizationId]);

CREATE INDEX [IX_Templates_OrganizationId_Name] ON [Templates] ([OrganizationId], [Name]);

CREATE INDEX [IX_Templates_OwnerUserId] ON [Templates] ([OwnerUserId]);

CREATE INDEX [IX_TemplateShares_SharedByUserId] ON [TemplateShares] ([SharedByUserId]);

CREATE INDEX [IX_TemplateShares_SharedWithDepartmentId] ON [TemplateShares] ([SharedWithDepartmentId]);

CREATE INDEX [IX_TemplateShares_SharedWithUserId] ON [TemplateShares] ([SharedWithUserId]);

CREATE INDEX [IX_TemplateShares_TemplateId] ON [TemplateShares] ([TemplateId]);

CREATE INDEX [IX_TemplateVersions_CreatedByUserId] ON [TemplateVersions] ([CreatedByUserId]);

CREATE UNIQUE INDEX [IX_TemplateVersions_TemplateId_VersionNumber] ON [TemplateVersions] ([TemplateId], [VersionNumber]);

CREATE INDEX [IX_UserPermissionProfiles_PermissionProfileId] ON [UserPermissionProfiles] ([PermissionProfileId]);

CREATE INDEX [IX_UserPermissionProfiles_UserId] ON [UserPermissionProfiles] ([UserId]);

CREATE INDEX [IX_UserRoles_RoleId] ON [UserRoles] ([RoleId]);

CREATE INDEX [IX_Users_DepartmentId] ON [Users] ([DepartmentId]);

CREATE UNIQUE INDEX [IX_Users_ExternalIdentityId] ON [Users] ([ExternalIdentityId]) WHERE [ExtenalIdentityId] is not null;

CREATE UNIQUE INDEX [IX_Users_Email] ON [Users] ([Email]);

CREATE INDEX [IX_Users_OrganizationId] ON [Users] ([OrganizationId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260920190637_InitialCreate', N'10.0.12');

COMMIT;
GO

