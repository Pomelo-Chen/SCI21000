using KGNetwork.Components.Account;
using KGNetwork.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using MudBlazor;
using System.Reflection.Metadata;
using VisNetwork.Blazor;
using VisNetwork.Blazor.Models;

namespace KGNetwork.Components.Pages
{
    public partial class ArticleDetail
    {
        [Parameter]
        public int Id { set; get; }


        [Inject]
        private ISnackbar Snackbar { get; set; }

        [Inject]
        private ArticleDBContext _context { set; get; }

        public Article Article { get; set; }

        public Graph Graph { set; get; }

        [Inject]
        IJSRuntime JSRuntime { set; get; }

        private string _graphString;


        private ApplicationUser user = default!;

        [Inject]
        IHttpContextAccessor HttpContextAccessor { set; get; } = default!;

        public string GraphString
        {
            get { return _graphString; }

            set
            {
                _graphString = value;

                dotString = @$"
                digraph {{
                            node [shape=box fontsize=12]
                            edge [color=gray, fontcolor=black]
                            {_graphString}
                        }}
";
            }
        }

        protected override void OnInitialized()
        {
            Article = _context.Articles.Find(Id);

        }

        protected override async Task OnInitializedAsync()
        {

            user = await UserAccessor.GetRequiredUserAsync(HttpContextAccessor.HttpContext);

            Graph = await _context.Graphs.Where(x => x.UT == Article.UT && x.UserId == user.Id).FirstOrDefaultAsync();

            if (Graph != null)
            {

                GraphString = Graph.Text;
            }


        }

        private Network network;

        private string errorMsg = string.Empty;

        private string dotString = string.Empty;

        private async Task NetworkReady() => await ParseDOT();

        private async Task OnClick() => await ParseDOT();

        private async Task ParseDOT()
        {
            errorMsg = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(dotString))
                {
                    var options = new NetworkOptions
                    {
                        AutoResize = true,
                        Width = "100%",
                        Height = "100%",
                        Physics = new PhysicsOptions()
                        {
                            Enabled = false
                        }

                    };

                    await network.ParseDOTNetwork(dotString);

                    await network.SetOptions(options);

                }


            }
            catch (Exception e)
            {
                errorMsg = e.ToString();
            }
        }

        private NetworkOptions NetworkOptions(Network network)
        {
            return new NetworkOptions
            {
                AutoResize = true,
                Nodes = new NodeOption
                {
                    BorderWidth = 1,
                    Shape = "box"
                }
            };
        }


        private async Task SaveGraph()
        {

            try
            {
                if (Graph != null)
                {

                    Graph.Text = GraphString;

                    await _context.SaveChangesAsync();

                    //保存成功

                    Snackbar.Add("保存成功");
                }
                else
                {
                    Graph = new Graph()
                    {
                        Text = GraphString,
                        Email = user.Email,
                        UserId = user.Id,
                        UT = Article.UT
                    };

                    _context.Graphs.Add(Graph);

                    await _context.SaveChangesAsync();

                    //插入成功
                    Snackbar.Add("保存成功");
                }
            }
            catch (Exception e)
            {

                Snackbar.Add("保存失败:" + e.ToString());
            }
        }
    }
}
