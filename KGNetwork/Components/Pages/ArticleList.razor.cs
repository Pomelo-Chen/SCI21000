using KGNetwork.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using MudBlazor;

namespace KGNetwork.Components.Pages
{
    public partial class ArticleList
    {
        [Inject]
        private ArticleDBContext _context { set; get; }

        public List<Article> Articles { get; set; }

        private bool _loading;

        private string _searchString;

        MudDataGrid<Article> dataGrid;

        private string DocumentType { set; get; }


        private IEnumerable<Article> pagedData;
        private MudTable<Article> table;

        protected override void OnInitialized()
        {
        }


        /// <summary>
        /// Here we simulate getting the paged, filtered and ordered data from the server
        /// </summary>
        private async Task<GridData<Article>> ServerReload(GridState<Article> state)
        {
            try
            {
                var data = from article in _context.Articles
                           select article;

                if (!string.IsNullOrEmpty(DocumentType))
                {
                    data = data.Where(s => s.DocumentType == DocumentType);
                }


                if (!string.IsNullOrWhiteSpace(_searchString) && !string.IsNullOrEmpty(_searchString))
                {
                    data = data.Where(s => s.ArticleTitle.Contains(_searchString)
                    || s.Source.Contains(_searchString)
                    || s.Authors.Contains(_searchString)
                    );
                }


                var totalItems = await data.CountAsync();

                var sortDefinition = state.SortDefinitions.FirstOrDefault();


                if (sortDefinition != null)
                {
                    switch (sortDefinition.SortBy)
                    {
                        case nameof(Article.TimesCited):
                            data = data.OrderByDirection(
                                sortDefinition.Descending ? SortDirection.Descending : SortDirection.Ascending,
                                o => o.TimesCited
                            );
                            break;

                        case nameof(Article.PublicationDate):
                            data = data.OrderByDirection(
                                sortDefinition.Descending ? SortDirection.Descending : SortDirection.Ascending,
                                o => o.PublicationDate
                            );
                            break;
                        default:
                            break;
                    }
                }

                var pagedData = await data.Skip(state.Page * state.PageSize).Take(state.PageSize).ToListAsync();
                return new GridData<Article>
                {
                    TotalItems = totalItems,
                    Items = pagedData
                };
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }

        private Task OnSearch(string text)
        {
            _searchString = text;
            return dataGrid.ReloadServerData();
        }

        private Task ValueChanged(string doctype)
        {
            DocumentType = doctype;
            return dataGrid.ReloadServerData();
        }







    }
}
