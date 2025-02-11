using Microsoft.AspNetCore.Mvc;

namespace Section6.Practice.Ctrls;
public class HomeController : Controller
{
    [Route("/")]
    public string Index()
    {
        return "Home index action";
    }

    [Route("/sq/{inp:double}")]
    public string square()
    {
        var inp = double.Parse(HttpContext.Request.RouteValues["inp"]?.ToString() ?? "0");
        return Math.Pow(inp, 2).ToString();
    }

    [Route("/unit-matrix/{n:int}")]
    public ContentResult unitMatrix()
    {
        int n = int.Parse(HttpContext.Request.RouteValues["n"]?.ToString() ?? "0");
        int[][] mat = new int[n][];
        for (int i = 0; i < n; i++)
            mat[i] = new int[n];
        for (int i = 0; i < n; i++)
            mat[i][i] = 1;

        string json = JsonConvert.SerializeObject(mat, Formatting.Indented);
        //var result = new ContentResult() { Content = json, ContentType = "application/json", StatusCode = 200 };
        var result = Content(json, "application/json");
        return result;
    }

    [Route("/unit-matrix2/{n:int}")]
    public JsonResult unitMatrix2()
    {
        int n = int.Parse(HttpContext.Request.RouteValues["n"]?.ToString() ?? "0");
        int[][] mat = new int[n][];
        for (int i = 0; i < n; i++)
            mat[i] = new int[n];
        for (int i = 0; i < n; i++)
            mat[i][i] = 1;

        var result = Json(mat);
        return result;
    }

    [Route("/gettext1")]
    public VirtualFileResult gettext1()
    {
        var result = File("sample1.txt", "plain/text", "sample1.txt");
        return result;
    }

    [Route("/getimage")]
    public VirtualFileResult getimage()
    {
        var result = File("sample.jpg", "image/jpeg", "sample.jpg");
        return result;
    }

    [Route("/gettext2")]
    public FileContentResult gettext2([FromServices]IWebHostEnvironment webHostEnvironment)
    {
        //Request
        //HttpContext.Request
        //ControllerContext.HttpContext.Request
        var path = System.IO.Path.Combine(webHostEnvironment.WebRootPath, "sample1.txt");
        var result = File(System.IO.File.ReadAllBytes(path), "plain/text", "sample1.txt");
        return result;
    }

}

