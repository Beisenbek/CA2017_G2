using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MyGallery.Models
{
    public interface IImageService
    {
        Task<UploadedImage> CreateUploadedImage(HttpPostedFileBase file);
        Task AddImageToBlobStorageAsync(UploadedImage image);
        List<BaseImage> GetImagesFromContainer();
        CloudTable CreateTable();
    }

    /*
    public class MyImageService : IImageService
    {
        public async Task<UploadedImage> CreateUploadedImage(HttpPostedFileBase file)
        {
            file.SaveAs(@"C:\work\kbtu2.png");
            return new UploadedImage();
        }
    }
    */
    public class ImageService : IImageService
    {
        private readonly string _imageRootPath;
        private readonly string _containerName;
        private readonly string _blobStorageConnectionString;

        public ImageService()
        {
            _imageRootPath = ConfigurationManager.AppSettings["ImageRootPath"];
            _containerName = ConfigurationManager.AppSettings["ImagesContainer"];
            _blobStorageConnectionString = ConfigurationManager.ConnectionStrings["BlobStorageConnectionString"].ConnectionString;
        }

        public List<BaseImage>  GetImagesFromContainer()
        {
            var container = this.GetImagesBlobContainer();
            List<BaseImage> res = new List<BaseImage>();

            foreach (var x in container.ListBlobs())
            {
                CloudBlockBlob blob = (CloudBlockBlob)x;
                string url = string.Format("{0}/{1}",_imageRootPath,blob.Name);
                BaseImage img = new BaseImage();
               // img.name = blob.Name;
              //  img.url = url;
                res.Add(img);
            }

            return res;
        }
        public async Task<UploadedImage> CreateUploadedImage(HttpPostedFileBase file)
        {
            if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
            {


                byte[] fileBytes = new byte[file.ContentLength];
                await file.InputStream.ReadAsync(fileBytes, 0, Convert.ToInt32(file.ContentLength));
                return new UploadedImage
                {
                    ContentType = file.ContentType,
                    Data = fileBytes,
                    Name = file.FileName,
                    // temporarily build a data url to return
                    Url = String.Format("data:image/jpeg;base64,{0}", Convert.ToBase64String(fileBytes))
                };
            }
            return null;
        }

        public async Task AddImageToBlobStorageAsync(UploadedImage image)
        {
            //  get the container reference
            var container = GetImagesBlobContainer();
            // using the container reference, get a block blob reference and set its type
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(image.Name);
            blockBlob.Properties.ContentType = image.ContentType;
            // finally, upload the image into blob storage using the block blob reference
            var fileBytes = image.Data;
            await blockBlob.UploadFromByteArrayAsync(fileBytes, 0, fileBytes.Length);
        }

        public CloudTable CreateTable()
        {
            // Retrieve the storage account from the connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(_blobStorageConnectionString);

            // Create the table client.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            // Retrieve a reference to the table.
            CloudTable table = tableClient.GetTableReference("imagetable");

            // Create the table if it doesn't exist.
            table.CreateIfNotExists();

            return table;
        }

        private CloudBlobContainer GetImagesBlobContainer()
        {
            // use the connection string to get the storage account
            var storageAccount = CloudStorageAccount.Parse(_blobStorageConnectionString);
            // using the storage account, create the blob client
            var blobClient = storageAccount.CreateCloudBlobClient();
            // finally, using the blob client, get a reference to our container
            var container = blobClient.GetContainerReference(_containerName);
            // if we had not created the container in the portal, this would automatically create it for us at run time
            container.CreateIfNotExists();
            // by default, blobs are private and would require your access key to download.
            //   You can allow public access to the blobs by making the container public.   
            container.SetPermissions(
                new BlobContainerPermissions
                {
                    PublicAccess = BlobContainerPublicAccessType.Blob
                });
            return container;
        }


    }
}