using Microsoft.AspNetCore.Mvc;

namespace GrpcService1.Controllers;

[ApiController, Route("[controller]")]
public class FilesController : ControllerBase
{
    [HttpGet()]
    public FileResult DownloadFile()  
    {  
        return PhysicalFile(@"D:\Playground\GrpcService1\GrpcService1\Protos\counter.proto", "text/plain", "counter.proto");  
    }  
}