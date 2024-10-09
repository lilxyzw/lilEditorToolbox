using System;
using System.IO;
using System.Linq;
using System.Text;

namespace jp.lilxyzw.editortoolbox
{
    internal static partial class DocsGeneratorMenu
    {
        private static void BuildIndex(string root, string code, Func<string,string> loc)
        {
            var langHeader = File.ReadAllText("docs_base/lang_header.ts");
            var sb = new StringBuilder();
            sb.AppendLine(langHeader);
            sb.AppendLine($"      {{ text: '{loc("Home")}', link: langName + '/' }},");
            sb.AppendLine($"      {{ text: '{loc("Document")}', link: langName + '/docs/', activeMatch: '/docs/' }}");
            sb.AppendLine($"    ],");
            sb.AppendLine($"    sidebar: [");

            // Document
            sb.AppendLine($"      {{");
            sb.AppendLine($"        text: '{loc("Document")}',");
            sb.AppendLine($"        link: langName + '/docs/',");
            sb.AppendLine($"        collapsed: false,");
            sb.AppendLine($"        items: [");
            // 設定は先に表示
            {
            var title = File.ReadLines(root+"/docs/Settings.md").First().Substring(2);
            sb.AppendLine($"          {{ text: '{title}', link: langName + '/docs/Settings' }},");
            }
            // Document - Types
            foreach(var file in Directory.GetFiles(root+"/docs", "*.md", SearchOption.TopDirectoryOnly))
            {
            if(file.EndsWith("index.md") || file.EndsWith("Settings.md")) continue;
            var title = File.ReadLines(file).First().Substring(2);
            sb.AppendLine($"          {{ text: '{title}', link: langName + '/docs/{Path.GetFileNameWithoutExtension(file)}' }},");
            }

            // Document - EditorWindow
            sb.AppendLine($"          {{");
            sb.AppendLine($"            text: '{loc("EditorWindow")}',");
            sb.AppendLine($"            items: [");
            foreach(var file in Directory.GetFiles(root+"/docs/EditorWindow", "*.md", SearchOption.TopDirectoryOnly))
            {
            if(file.EndsWith("index.md")) continue;
            var title = File.ReadLines(file).First().Substring(2);
            sb.AppendLine($"              {{ text: '{title}', link: langName + '/docs/EditorWindow/{Path.GetFileNameWithoutExtension(file)}' }},");
            }
            sb.AppendLine($"            ]");
            sb.AppendLine($"          }},");

            // Document - Components
            sb.AppendLine($"          {{");
            sb.AppendLine($"            text: '{loc("Components")}',");
            sb.AppendLine($"            items: [");
            foreach(var file in Directory.GetFiles(root+"/docs/Components", "*.md", SearchOption.TopDirectoryOnly))
            {
            if(file.EndsWith("index.md")) continue;
            var title = File.ReadLines(file).First().Substring(2);
            sb.AppendLine($"              {{ text: '{title}', link: langName + '/docs/Components/{Path.GetFileNameWithoutExtension(file)}' }},");
            }
            sb.AppendLine($"            ]");
            sb.AppendLine($"          }},");

            sb.AppendLine($"        ]");
            sb.AppendLine($"      }},");

            var langFooter = File.ReadAllText("docs_base/lang_footer.ts");
            sb.Append(langFooter);
            sb.Replace("LANG", code);

            sb.Replace("TO_SELECT", loc("to select"));
            sb.Replace("ENTER", loc("enter"));
            sb.Replace("TO_NAVIGATE", loc("to navigate"));
            sb.Replace("UP_ARROW", loc("up arrow"));
            sb.Replace("DOWN_ARROW", loc("down arrow"));
            sb.Replace("TO_CLOSE", loc("to close"));
            sb.Replace("ESCAPE", loc("escape"));

            sb.Replace("SEARCH", loc("Search"));
            sb.Replace("DETAIL", loc("Display detailed list"));
            sb.Replace("RESET", loc("Reset search"));
            sb.Replace("CLOSE", loc("Close search"));
            sb.Replace("NO_RESULTS", loc("No results for"));

            sb.Replace("UPDATE_AT", loc("Updated at"));

            WriteText($"docs/.vitepress/config/{code}.ts", sb.ToString());
        }
    }
}
