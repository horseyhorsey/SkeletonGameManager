# A wrapper for running a pyprocgame, hijack logging to game directory and still view the console window.
# Packed as sg_runner.exe in the main WPF application.
# Build events
	xcopy "$(TargetPath)"  "$(SolutionDir)UI\SkeletonGameManager.WPF\Tools"