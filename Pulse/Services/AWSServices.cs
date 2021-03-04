using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using ModernHttpClient;
using Xamarin.Forms;

namespace Pulse
{
	public class AWSServices
	{
		readonly HttpClient client;

		public AWSServices()
		{
			client =Device.RuntimePlatform == Device.iOS ? new HttpClient() : new HttpClient(new NativeMessageHandler());
			client.MaxResponseContentBufferSize = Constant.MaxResponseContentBufferSize;
		}

		async public Task<bool> UploadAWSFile(Stream imageStream, string folderName, string fileName)
		{
			try
			{
				if (imageStream != null && !string.IsNullOrEmpty(fileName))
				{
					var region = RegionEndpoint.GetBySystemName(App.AWSCurrentDetails.response.s3_region);
					IAmazonS3 S3Client = new AmazonS3Client(App.AWSCurrentDetails.response.aws_access_key, App.AWSCurrentDetails.response.aws_secret_key, region);
                    TransferUtilityUploadRequest transferUtilityRequest = new TransferUtilityUploadRequest
					{
						BucketName = App.AWSCurrentDetails.response.s3_bucket,
						InputStream = imageStream,
						Key = folderName + "/" + fileName,
						CannedACL = S3CannedACL.PublicRead
					};
					TransferUtility fileTransferUtility = new TransferUtility(S3Client);
                    await fileTransferUtility.UploadAsync(transferUtilityRequest, default(System.Threading.CancellationToken));
					return true;
				}
				return false;
			}
			catch (TaskCanceledException)
			{
				return false;
			}
		}
		async public Task<bool> DeleteAWSFile(string filename)
		{
			try
			{
				if (!string.IsNullOrEmpty(filename))
				{
					IAmazonS3 S3Client = new AmazonS3Client(App.AWSCurrentDetails.response.aws_access_key, App.AWSCurrentDetails.response.aws_secret_key, App.AWSCurrentDetails.response.s3_region);
					Amazon.S3.Model.DeleteObjectRequest deleteRequest = new Amazon.S3.Model.DeleteObjectRequest
					{
						BucketName = App.AWSCurrentDetails.response.s3_bucket,
						Key = filename
					};
					await S3Client.DeleteObjectAsync(deleteRequest, default(System.Threading.CancellationToken));
					return true;
				}
				return false;
			}
			catch (TaskCanceledException)
			{
				return false;
			}
		}

		async public Task<bool> UploadAWSFilePathOneByOne(string filePath, string folderName, string fileName)
		{
			try
			{

				var region = RegionEndpoint.GetBySystemName(App.AWSCurrentDetails.response.s3_region);
				IAmazonS3 S3Client = new AmazonS3Client(App.AWSCurrentDetails.response.aws_access_key, App.AWSCurrentDetails.response.aws_secret_key, region);


				TransferUtilityUploadRequest transferUtilityRequest = new TransferUtilityUploadRequest
				{
					BucketName = App.AWSCurrentDetails.response.s3_bucket,
					FilePath = filePath,
					Key = folderName + "/" + fileName,
					CannedACL = S3CannedACL.PublicRead
				};
				TransferUtility fileTransferUtility = new TransferUtility(S3Client);
				await fileTransferUtility.UploadAsync(transferUtilityRequest, default(System.Threading.CancellationToken));
				return true;
			}
			catch (TaskCanceledException)
			{
				return false;
			}
		}

		async public Task<bool> UploadAWSFilePath(ObservableCollection<MediaData> selectedMediaList)
		{
			try
			{
				if (selectedMediaList != null && selectedMediaList.Count > 0)
				{
					var region = RegionEndpoint.GetBySystemName(App.AWSCurrentDetails.response.s3_region);
					IAmazonS3 S3Client = new AmazonS3Client(App.AWSCurrentDetails.response.aws_access_key, App.AWSCurrentDetails.response.aws_secret_key, region);
					foreach (var i in selectedMediaList)
					{
						if (!string.IsNullOrEmpty(i.FileName) && i.FilePath != null)
						{
							var folder = i.FileType == 0 ? App.AWSCurrentDetails.response.images_path.event_images : App.AWSCurrentDetails.response.images_path.event_videos;
							var videoStream = i.MediaVideoBytes != null && i.MediaVideoBytes.Length > 0 ? new MemoryStream(i.MediaVideoBytes) : null;
							TransferUtilityUploadRequest transferUtilityRequest = new TransferUtilityUploadRequest
							{
								BucketName = App.AWSCurrentDetails.response.s3_bucket,
								Key = folder + "/" + i.FileName,
								CannedACL = S3CannedACL.PublicRead
							};
							if (i.MediaVideoBytes != null && i.MediaVideoBytes.Length > 0)
							{
								transferUtilityRequest.InputStream = videoStream;
							}
							else
							{
								transferUtilityRequest.FilePath = i.FilePath;
							}

							TransferUtility fileTransferUtility = new TransferUtility(S3Client);
							await fileTransferUtility.UploadAsync(transferUtilityRequest, default(System.Threading.CancellationToken));
						}
				}
					return true;
				}
				return false;
			}
			catch (TaskCanceledException e)
			{
				return false;
			}
		}
	}
}
