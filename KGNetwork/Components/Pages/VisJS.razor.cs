using Microsoft.AspNetCore.Http;
using MudBlazor;
using VisNetwork.Blazor;
using VisNetwork.Blazor.Models;

namespace KGNetwork.Components.Pages
{
    public partial class VisJS
    {
        private Network network;

        private string errorMsg = string.Empty;


        private string _graphString;
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
            GraphString = @"

""理解型学习""->""通过理解其含义""[label=""字面上意为""]->""未知联系已知，新学联系已学""[label=""指的是""];
""理解型学习""->""一种学习方式""[label=""是""]->""产生和化解认知冲突""[label=""通过""];
""产生和化解认知冲突""->""人类知识高速公路上以高层知识生成器为目标的理解型学习""[label=""由...实践""]
""未知联系已知，新学联系已学""->""概念形成：从经验中提取出概念""[label=""通过""]->""概念网络（命题网络)""[label=""构建""];
""未知联系已知，新学联系已学""->""概念同化：基于已有概念形成新概念""[label=""通过""]->""概念网络（命题网络)""[label=""构建""];
""通过理解其含义""->""概念""[label=""来学习""]->""事物""[label=""表示""];
""概念""->""概念间联系""[label=""通过""]->""概念网络（命题网络)""[label=""形成""];
""通过理解其含义""->""命题""[label=""来学习""]->""命题间联系""[label=""通过""]->""概念网络（命题网络)""[label=""形成""];
""概念间联系""->""命题""[label=""形成""];
";
        }

        private string dotString { set; get; }


        private async Task NetworkReady() => await ParseDOT();

        private async Task OnClick() => await ParseDOT();

        private async Task ParseDOT()
        {
            errorMsg = string.Empty;
            try
            {
                var options = new NetworkOptions
                {
                    AutoResize = true,
                    Width = "100%",
                    Height = "100%",
                    Physics = new PhysicsOptions()
                    {
                        Enabled = false
                    },
                };



                await network.ParseDOTNetwork(dotString);

                await network.SetOptions(options);


                errorMsg = "OK";
            }
            catch (Exception e)
            {
                errorMsg = e.ToString();
                Console.WriteLine(e.Message);
            }
        }
    }
}
