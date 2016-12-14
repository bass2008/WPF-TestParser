using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using ResumeParser.Common.Extensions;
using ResumeParser.Domain.Enums;
using ResumeParser.Domain.Models;
using TestParse.Helpers.Log;

namespace TestParse.Helpers.Parser
{
    public class Parser : IParser
    {
        private readonly string _host;

        private readonly string _url;

        private readonly string _pageParam;

        private readonly ILogger _logger;

        public Parser(ILogger logger, string host, string url, string pageParam)
        {
            _logger = logger;
            _pageParam = pageParam;
            _host = host;
            _url = url;
        }

        public async Task<List<Resume>> GetResumes()
        {
            var url = $"{_host}{_url}{_pageParam}";
            var result = new List<Resume>();

            var page = 0;
            _logger.Message($"Страница {page + 1}...");
            var resumes = await GetResumesByPage(url + page);
            result.AddRange(resumes);

            while (resumes.Count != 0)
            {
                page++;
                _logger.Message($"Страница {page + 1}...");
                resumes = await GetResumesByPage(url + page);
                result.AddRange(resumes);
            }

            return result;
        }

        private async Task<List<Resume>> GetResumesByPage(string url)
        {
            var result = new List<Task<Resume>>();
            using (var client = new WebClient { Encoding = Encoding.UTF8 })
            {
                var doc = new HtmlDocument();
                var response = await client.DownloadStringTaskAsync(url);
                doc.LoadHtml(response);

                var innerDoc = new HtmlDocument();

                var resultNode = doc.DocumentNode.SelectSingleNode("//table[@class='output']");

                if (resultNode == null)
                    return new List<Resume>();

                foreach (var node in resultNode.ChildNodes.Where(x => x.Name == "tr").ToList())
                {
                    innerDoc.LoadHtml(node.InnerHtml);
                    var resumeUrl = GetResumeUrl(innerDoc);
                    var resumeFullUrl = $"{_host}{resumeUrl}";
                    var resume = ParseResumeFromUrl(resumeFullUrl);
                    result.Add(resume);
                }
            }
            var done = await Task.WhenAll(result);
            return done.ToList();
        }
        
        private static string GetResumeUrl(HtmlDocument innerDoc)
            =>
                innerDoc.DocumentNode.SelectSingleNode("/td[2]/div[2]/span[1]/a[1]")
                    .Attributes.First(x => x.Name == "href")
                    .Value;

        private static async Task<Resume> ParseResumeFromUrl(string resumeFullUrl)
        {
            var innerDoc = new HtmlDocument();
            var client = new WebClient {Encoding = Encoding.UTF8};
            var response = await client.DownloadStringTaskAsync(resumeFullUrl);
            innerDoc.LoadHtml(response);

            var sex = innerDoc.DocumentNode.SelectSingleNode("//div[@class='resume-header-block']/p[1]/span[1]")?.InnerText;
            var age = innerDoc.DocumentNode.SelectSingleNode("//div[@class='resume-header-block']/p[1]/span[2]")?.InnerText;
            var resumeName = innerDoc.DocumentNode.SelectSingleNode("//h2[@class='header header_level-2']/span[1]")?.InnerText;
            var salary = innerDoc.DocumentNode.SelectSingleNode("//span[@class='resume-block__salary']")?.InnerText;
            var currency = string.Empty;

            if (age.IsNotNullOrWhiteSpace())
            {
                var splited = age.Split();
                age = splited[0];
            }

            if (salary.IsNotNullOrWhiteSpace())
            {
                var splited = salary.Split();
                salary = splited[0];
                currency = splited[1];
            }

            var resume = new Resume
            {
                Salary = salary.IsNotNullOrWhiteSpace() ? int.Parse(salary) : 0,
                Currency = currency,
                Sex = sex == "Male" || sex == "Мужчина" ? Sex.Male : Sex.Female,
                Age = age.IsNotNullOrWhiteSpace() ? int.Parse(age) : 0,
                Name = resumeName,
                Url = resumeFullUrl
            };
            return resume;
        }
    }
}
