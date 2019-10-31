namespace Hedwig
{
    public static class ErrorMessages
    {
        private static string NOT_FOUND_TMPL = "{0} with id {1} not found";
        public static string NOT_FOUND(string type, int id)
        {
            return string.Format(NOT_FOUND_TMPL, type, id);
        }

        private static string USER_CANNOT_ACCESS_ENTITY_TMPL = "Current user cannot access {0}";
        public static string USER_CANNOT_ACCESS_ENTITY(string type)
        {
            return string.Format(USER_CANNOT_ACCESS_ENTITY_TMPL, type);
        }
    }
}
