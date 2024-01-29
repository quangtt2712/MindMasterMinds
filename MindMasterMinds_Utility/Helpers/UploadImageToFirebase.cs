using Firebase.Storage;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindMasterMinds_Utility.Helpers
{
    public class UploadImageToFirebase
    {
        public static async Task<string> UploadProductImageToFirebase(IFormFile file)
        {
            var storage = new FirebaseStorage("mindmasterminds.appspot.com");
            var imageName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var imageUrl = await storage.Child("images")
                                        .Child(imageName)
                                        .PutAsync(file.OpenReadStream());
            return imageUrl;
        }
    }
}
