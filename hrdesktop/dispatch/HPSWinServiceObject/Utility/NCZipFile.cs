using System;
using System.IO;

using ICSharpCode.SharpZipLib.Checksums;
using ICSharpCode.SharpZipLib.Zip;


namespace NC.HPS.Lib
{
	/// <summary>
	/// clsZipFile µÄÕªÒªËµÃ÷¡£
	/// </summary>
	public class NCZipFile
	{
		private static string TemporaryDirectory = System.Environment.CurrentDirectory + @"\TMP";

        public NCZipFile()
		{
			//
			// TODO: ÔÚ´Ë´¦ÌúØÓ¹¹ÔE¯ÊıÂß¼­
			//
		}

		//½âÑ¹ËõÎÄ¼ş
		public static void UnZipFile(string file)
		{
			ZipInputStream s = new ZipInputStream(File.OpenRead(AppDomain.CurrentDomain.BaseDirectory + "\\" + file));
		
			ZipEntry theEntry;
			try
			{
				//Ñ­»·»ñÈ¡Ñ¹ËõÊµÌE
				while ((theEntry = s.GetNextEntry()) != null) 
				{
					//string directoryName = Path.GetDirectoryName(theEntry.Name);
					string fileName      = Path.GetFileName(theEntry.Name);
			
					if (fileName != String.Empty 
						&& fileName.IndexOf("AutoUpdate") < 0
						&& fileName.IndexOf("SharpZipLib") < 0) 
					{
						if(File.Exists(fileName))
						{
							try
							{
								File.Delete(fileName);
							}
							catch
							{}
						}

						FileStream streamWriter = File.Create(fileName);
				
						try
						{
							int size = 2048;
							byte[] data = new byte[2048];
							while (true) 
							{
								size = s.Read(data, 0, data.Length);
								if (size > 0) 
								{
									//Ğ´ÎÄ¼ş
									streamWriter.Write(data, 0, size);
								} 
								else 
								{
									break;
								}
							}
						}
						catch
						{
						}
						finally
						{
							streamWriter.Close();
						}
					}
					
//					//½ø¶ÈÌõÀÛ¼Ó
//					if(pgbDownload.Value < 100)
//					{
//						pgbDownload.Value += 1;
//
//
//						pgbDownload.Refresh();
//					}

					System.Threading.Thread.Sleep(100);
				}
			}
			catch(Exception ex)
			{
				throw ex;
			}
			finally
			{
				s.Close();
			}
		}
		/// <summary>
		/// Ñ¹ËõÖ¸¶¨Ä¿Â¼ÄÚµÄÎÄ¼ş
		/// </summary>
		/// <param name="pathName">Ä¿Â¼Â·¾¶</param>
		/// <param name="comparedFileName">Ñ¹ËõºóµÄÑ¹ËõÎÄ¼şÃE/param>
		/// <param name="compsLevel">Ñ¹ËõË®Æ½</param>
		public static void ZipFiles(string pathName,string comparedFileName,int compsLevel)
		{
			string[] filenames = Directory.GetFiles(pathName);
		
			Crc32 crc = new Crc32();

			if(File.Exists(comparedFileName))
			{
				File.Delete(comparedFileName);
			}
			if(Directory.Exists(TemporaryDirectory))
			{
				Directory.Delete(TemporaryDirectory,true);
			}
			
			Directory.CreateDirectory(TemporaryDirectory);
			

			ZipOutputStream s = new ZipOutputStream(File.Create(comparedFileName));

			s.SetLevel(compsLevel); // 0 - store only to 9 - means best compression
		
			try
			{
				

				foreach (string file in filenames) 
				{
					if(file.IndexOf(comparedFileName) >= 0)
					{
						continue;
					}
					string fileName = file.Remove(0,file.LastIndexOf(@"\") + 1);
					fileName = TemporaryDirectory + @"\" + fileName;

					
					File.SetAttributes(file,FileAttributes.Normal);

					File.Copy(file,
						fileName,true);

					FileStream fs = File.OpenRead(fileName);
			
					byte[] buffer = new byte[fs.Length];
					fs.Read(buffer, 0, buffer.Length);
					ZipEntry entry = new ZipEntry(file);
			
					entry.DateTime = (new FileInfo(file)).LastWriteTime;
			
					// set Size and the crc, because the information
					// about the size and crc should be stored in the header
					// if it is not set it is automatically written in the footer.
					// (in this case size == crc == -1 in the header)
					// Some ZIP programs have problems with zip files that don't store
					// the size and crc in the header.
					entry.Size = fs.Length;
					fs.Close();
			
					crc.Reset();
					crc.Update(buffer);
			
					entry.Crc  = crc.Value;

					s.PutNextEntry(entry);
			
					s.Write(buffer, 0, buffer.Length);

					File.Delete(fileName);

				}

				if(Directory.Exists(TemporaryDirectory))
				{
					Directory.Delete(TemporaryDirectory,true);
				}

			}
			catch(Exception ex)
			{
				throw ex;
			}
			finally
			{
				s.Finish();
				s.Close();
			}
		}
	}
}
