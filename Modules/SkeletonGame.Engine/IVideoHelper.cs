using System;
using System.Diagnostics;
using System.IO;

namespace SkeletonGame.Engine
{
    //public interface IVideoHelper
    //{
    //    string CreateIncrementalFileName(string inputFile);
    //    void TrimVideoRange(string ffmpeg, string inputFile, string outputFile, TimeSpan start, TimeSpan end);        
    //}

    public static class VideoHelper // : IVideoHelper
    {
        public static string AudioExportFolder = @"";
        public static string VideoExportFolder = @"";

        public static void TrimVideoRange(string ffmpeg, string inputFile, string outputFile, TimeSpan start, TimeSpan end)
        {
            var startStr = start.ToString(@"hh\:mm\:ss");
            var endStr = end.ToString(@"hh\:mm\:ss");
            var startInfo = new ProcessStartInfo(ffmpeg + "\\ffmpeg.exe");
            string outputNewFile = CreateIncrementalFileName(inputFile);

            inputFile = "\"" + inputFile + "\"";
            outputNewFile = "\"" + outputNewFile + "\"";


            //startInfo.Arguments = $"-i {inputFile} -vcodec copy -acodec copy -ss {start.ToString()} -to {end.ToString()} {outputNewFile}";

            //Transcode
            startInfo.Arguments = $"-ss {start.ToString()} -i {inputFile} -vcodec copy -acodec copy -t {(end - start).ToString()} {outputNewFile}";

            //ffmpeg - ss 00:08:00 - i Video.mp4 - ss 00:01:00 - t 00:01:00 - c copy VideoClip.mp4
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;
            Process.Start(startInfo).WaitForExit();
        }

        public static void TrimVideoRange(string ffmpegPath, string input, string outfileName, TimeSpan start, TimeSpan end, bool exportVideo = true, bool exportAudio = true, string preset = "Medium")
        {
            //ffmpeg - i C:\temp\potc.mp4 - map 0:v - vcodec copy c:\temp\sep.mp4 - map 0:a -ab 256k c:\temp\sep.ogg

            var startInfo = new ProcessStartInfo(@"C:\FFMpeg\x86\ffmpeg.exe");

            input = "\"" + input + "\"";
            var outaudiofileName = "\"" + Path.Combine(AudioExportFolder, outfileName + ".ogg") + "\"";
            var outvideofileName = "\"" + Path.Combine(VideoExportFolder, outfileName + ".mp4") + "\"";

            if (exportVideo && exportAudio)
                startInfo.Arguments = $"-i {input} -map 0:v -ss {start.ToString(@"hh\:mm\:ss\.ms")} -t {(end - start).ToString()} -c:v libx264 -preset {preset} {outvideofileName} -map 0:a -ss {start.ToString(@"hh\:mm\:ss\.ms")} -t {(end - start).ToString()} {outaudiofileName}";
            //Export no audio
            else if (exportVideo)
            {
                startInfo.Arguments = $"-i {input} -map 0:v -ss {start.ToString(@"hh\:mm\:ss\.ms")} -t {(end - start).ToString()} -c:v libx264 -preset {preset} {outvideofileName} -an";
            }
            else if (exportAudio)
            {
                startInfo.Arguments = $"-i {input} -vn -map 0:a -ss {start.ToString(@"hh\:mm\:ss\.ms")} -t {(end - start).ToString()} {outaudiofileName}";
            }

            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;
            Process.Start(startInfo).WaitForExit();
        }

        /// <summary>
        /// Converts (reencodes) a movie clip by taking an amount of frames from a frames starting position. <para/> Note: Need to set the static VideoExportFolder
        /// </summary>
        /// <param name="ffmpeg">The ffmpeg executable</param>
        /// <param name="inputFile">The input file.</param>
        /// <param name="outputFile">The output file.</param>
        /// <param name="startTime">The start frame.</param>
        /// <param name="frames">The frames.</param>
        /// <param name="frameRate">The frame rate.</param>
        /// <param name="preset">The ffmpeg preset.</param>
        /// <returns></returns>
        public static void ConvertVideoClip(string ffmpeg, string inputFile, string outputFile, TimeSpan startTime, TimeSpan endTime, double frameRate, string resize = null, string preset = "Medium")
        {
            var startInfo = new ProcessStartInfo(ffmpeg);

            //Wrap in quotes in the case of spaces
            var input = "\"" + inputFile + "\"";            
            var outvideofileName = "\"" + outputFile + "\"";

            //Get the -ss start frame
            var metaDataRemove = $"-map_metadata -1 -map_chapters -1";
            //var start = startFrame / frameRate;

            //Are we resizing the clip
            string size = resize == null ? string.Empty : $"-s {resize}";

            //Join args to convert
            var ffmpegArgs = $"-ss {startTime} -to {endTime} -i {input} {metaDataRemove} -c:v libx264 -preset {preset} {size} {outvideofileName}";

            //Start the process
            startInfo.Arguments = ffmpegArgs;
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;
            Process.Start(startInfo).WaitForExit();
        }

        public static void ConvertAudioClip(string ffmpeg, string file, string tempOutputFile, int startFrame, int frames, double frameRate)
        {
            var startInfo = new ProcessStartInfo(ffmpeg);

            var input = "\"" + file + "\"";
            var outaudiofileName = "\"" + tempOutputFile + "\"";

            //Get the -ss start frame
            var metaDataRemove = $"-map_metadata -1 -map_chapters -1";
            var start = startFrame / frameRate;

            var ffmpegArgs = $"-ss {start} -i {input} -frames {frames} {metaDataRemove} -vn {outaudiofileName}";

            RunFfmpeg(startInfo, ffmpegArgs);

            //Copy to the assets directory
            File.Copy(tempOutputFile, Path.Combine(AudioExportFolder, Path.GetFileName(tempOutputFile)));
        }

        public static void SplitAudioAndVideo(string ffmpeg, string inputFile, string outputFileName, bool exportVideo = true, bool exportAudio =true, string audioExt = "ogg")
        {
            var startInfo = new ProcessStartInfo(ffmpeg);            

            string ffmpegArgs = null;

            var input = "\"" + inputFile + "\"";

            if (exportAudio)
            {
                //ffmpeg - i video.mp4 - vn - ab 256 audio.mp3
                var audioFile = Path.Combine(AudioExportFolder, outputFileName + "." + audioExt);
                var outputAudioFileName = "\"" + audioFile + "\"";
                ffmpegArgs = $"-i {input} -vn {outputAudioFileName}";

                RunFfmpeg(startInfo, ffmpegArgs);                
            }
            
            if (exportVideo)
            {
                var videoFile = Path.Combine(VideoExportFolder, outputFileName + ".mp4");
                var outputVideoFileName = "\"" + videoFile + "\"";
                ffmpegArgs = $"-i {input} -c:v copy -an {outputVideoFileName}";

                RunFfmpeg(startInfo, ffmpegArgs);
            }
        }

        private static void RunFfmpeg(ProcessStartInfo startinfo, string args)
        {
            startinfo.Arguments = args;
            startinfo.UseShellExecute = false;
            startinfo.CreateNoWindow = true;
            Process.Start(startinfo).WaitForExit();
        }

        public static string CreateIncrementalFileName(string inputFile)
        {
            int i = 0;
            var fullPath = Path.GetDirectoryName(inputFile);
            var name = Path.GetFileName(inputFile);
            var ext = Path.GetExtension(name);
            var nameNoExt = Path.GetFileNameWithoutExtension(name);
            var outputNewFile = fullPath + "\\" + name;

            if (File.Exists(outputNewFile))
            {
                var incrementout = nameNoExt + $"_{i}{ext}";
                while (File.Exists(fullPath + "\\" + incrementout))
                {
                    i++;
                    incrementout = nameNoExt + $"_{i}{ext}";
                }

                outputNewFile = fullPath + "\\" + incrementout;
            }

            return outputNewFile;
        }
    }
}
