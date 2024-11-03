namespace KGNetwork.Utils
{
    public static class AuthorHelper
    {
        public static string KeepTwoAuthors(string authors)
        {
            if (string.IsNullOrEmpty(authors))
            {
                return authors;
            }

            if (!authors.Contains(';'))
            {
                return authors;
            }

            var terms = authors.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);

            return terms[0] + "; " + terms[1];

        }

        

        public static List<string> GetList(string authors)
        {
            if (string.IsNullOrEmpty(authors) || string.IsNullOrWhiteSpace(authors))
            {
                return new List<string>();
            }

            var terms = authors.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);

            return new List<string>(terms);

        }
    }
}