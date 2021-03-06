﻿namespace ProcessingTools.Constants.Web
{
    public sealed class ViewNames
    {
        public const string DefaultLayoutViewName = "~/Views/Shared/_Layout.cshtml";

        public const string LoginPartialViewName = "_LoginPartial";
        public const string EditPartialViewName = "_Edit";
        public const string DetailsPartialViewName = "_Details";

        public const string DetailsHeaderPartialViewName = "_DetailsHeader";
        public const string DetailsFooterPartialViewName = "_DetailsFooter";

        public const string DeleteHeaderPartialViewName = "_DeleteHeader";
        public const string DeleteFooterPartialViewName = "_DeleteFooter";

        public const string EditHeaderPartialViewName = "_EditHeader";
        public const string EditFooterPartialViewName = "_EditFooter";

        public const string HelpSubmenuPartialViewName = "_HelpSubmenu";

        public const string HelpViewName = "Help";
        public const string DefaultHelpViewName = "~/Views/Shared/Help.cshtml";
        public const string BackToListFooterPartialViewName = "_BackToListFooter";

        public const string NavigationFootPartialViewName = "~/Views/Shared/_NavigationFoot.cshtml";

        public const string BadRequestErrorViewName = "~/Views/Shared/Errors/BadRequest.cshtml";
        public const string ErrorViewName = "~/Views/Shared/Errors/Error.cshtml";
        public const string ErrorFootPartialViewName = "~/Views/Shared/Errors/_ErrorFoot.cshtml";
        public const string InvalidActionErrorViewName = "~/Views/Shared/Errors/InvalidAction.cshtml";
        public const string NotFoundErrorViewName = "~/Views/Shared/Errors/NotFound.cshtml";
        public const string InvalidNumberOfItemsPerPageErrorViewName = "~/Views/Shared/Errors/InvalidNumberOfItemsPerPage.cshtml";
        public const string InvalidOrEmptyFilesErrorViewName = "~/Views/Shared/Errors/InvalidOrEmptyFiles.cshtml";
        public const string InvalidPageNumberErrorViewName = "~/Views/Shared/Errors/InvalidPageNumber.cshtml";
        public const string NoFilesSelectedErrorViewName = "~/Views/Shared/Errors/NoFilesSelected.cshtml";
        public const string InvalidIdErrorViewName = "~/Views/Shared/Errors/InvalidId.cshtml";
        public const string InvalidUserIdErrorViewName = "~/Views/Shared/Errors/InvalidUserId.cshtml";
    }
}
