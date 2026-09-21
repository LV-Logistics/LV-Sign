using System;
using System.Collections.Generic;
using System.Text;

namespace LV.SignFlow.Domain.Authorization
{
    public class PermissionCodes
    {
        public static class Envelopes
        {
            public const string View = "Envelope.View";
            public const string Create = "Envelope.Create";
            public const string Send = "Envelope.Send";
            public const string Void = "Envelope.Void";
            public const string Delete = "Envelope.Delete";
        }

        public static class Templates
        {
            public const string View = "Template.View";
            public const string Create = "Template.Create";
            public const string Edit = "Template.Edit";
            public const string Delete = "Template.Delete";
            public const string Use = "Template.Use";
            public const string Share = "Template.Share";
        }

        public static class Users
        {
            public const string View = "User.View";
            public const string Manage = "User.Manage";
        }

        public static class Roles
        {
            public const string View = "Role.View";
            public const string Manage = "Role.Manage";
        }

        public static class PermissionProfiles
        {
            public const string View = "PermissionProfile.View";
            public const string Manage = "PermissionProfile.Manage";
        }

        public static class Reports
        {
            public const string View = "Report.View";
        }

        public static class Audit
        {
            public const string View = "Audit.View";
        }

        public static class Stamps
        {
            public const string View = "Stamp.View";
            public const string Manage = "Stamp.Manage";
        }
    }
    }

