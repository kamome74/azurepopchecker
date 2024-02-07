using Azure.Monitor.Query.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualBasic.FileIO;

namespace azurepopchecker.Pages
{
	public class QueryResult
	{
		public string agent = "";
		public string XAzureRef = "";
		public string clientcountry = "";
		public string clientIP = "";
		public string pop = "";
		public string popLocation = "";
		public string popRegion = "";
		public string message = "";
	}

	struct popInfo
	{
		public string popLoc;
		public string popRegion;
	}
	public class IndexModel : PageModel
	{
		bool isPost = false;
		QueryResult res = null;

		private readonly ILogger<IndexModel> _logger;

		public IndexModel(ILogger<IndexModel> logger)
		{
			_logger = logger;
		}

		public bool IsPost() { return isPost; }
		public QueryResult getResult() { return res; }

		Dictionary<string, popInfo> loadCSV(string filename)
		{
			FileStream fi = new FileStream(filename, FileMode.Open, FileAccess.Read);
			StreamReader si = new StreamReader(fi, System.Text.Encoding.UTF8);

			Dictionary<string, popInfo> table = new Dictionary<string, popInfo>();

			string str;
			while (!si.EndOfStream)
			{
				str = si.ReadLine();

				TextFieldParser parser = new TextFieldParser(new StringReader(str));
				parser.HasFieldsEnclosedInQuotes = true;
				parser.SetDelimiters(",");

				string[] buf = parser.ReadFields();

				popInfo pop = new popInfo();
				pop.popLoc = buf[1];
				pop.popRegion = buf[2];
				table.Add(buf[0], pop);
			}

			si.Close();

			return table;
		}

		public void OnGet()
		{

		}

		public void OnPost()
		{
			isPost = true;

			string trackingRef = Request.Form["XAzureRef"];

			string tenantID = "Your Azure Entra Tenant ID";
			string clientID = "Your Registered Application's Client ID";
			string clientSecret = "Your Registered Application's Key Value";

			var credential = new Azure.Identity.ClientSecretCredential(tenantID, clientID, clientSecret);
			var logAnalyticsClient = new Azure.Monitor.Query.LogsQueryClient(credential);

			string querystring = "AzureDiagnostics | where Category == \"FrontDoorAccessLog\" and trackingReference_s == \"" + trackingRef + "\"";

			string analyticsSpaceId = "Your Log Analytics Workspace ID";

			Azure.Response<Azure.Monitor.Query.Models.LogsQueryResult> response = logAnalyticsClient.QueryWorkspace(
					analyticsSpaceId,
					querystring,
					new Azure.Monitor.Query.QueryTimeRange(TimeSpan.FromDays(1)));

			LogsTable table = response.Value.Table;
			res = new QueryResult();
			Dictionary<string, popInfo> poplist = loadCSV("Pages/data/poplist.csv");

			if (table.Rows.Count == 0)
				res.message = "Nope. Nothing";
			else
			{
				var pop_s = table.Rows.First()["pop_s"].ToString();

				res.agent = table.Rows.First()["userAgent_s"].ToString();
				res.XAzureRef = table.Rows.First()["trackingReference_s"].ToString();
				res.clientcountry = table.Rows.First()["clientCountry_s"].ToString();
				res.clientIP = table.Rows.First()["clientIp_s"].ToString();
				res.pop = pop_s;
				res.popLocation = poplist[pop_s].popLoc;
				res.popRegion = poplist[pop_s].popRegion;
			}
		}
	}
}