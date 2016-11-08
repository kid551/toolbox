# SAFS Reference Generation Debug

In order to debug the SAFS Reference Generation locally, you need to:

1. Delete the *push to GitHub and SourceForge* parts of `update.safs.reference` in safs.build.xml, which has been done in this folder.
2. As we use the local repository of SAFS project instead of downloading from GitHub, we need to rename the 'Core' as 'safsdev.core', 'build' as 'safsdev.build', 'SAFS-Android-Remote-Control' as 'droid.remotecontrol', 'SAFS-Android-Engine' as 'droid.engine', 'SAFS-Android-Messenger' as 'droid.messenger'.
3. Run the `BuildRefAutomation.bat`.
4. Run the `ant update.safs.reference` in CMD.

Then, you can find the generated docs in `.\source\common\doc`, where you can do your modifications.

After you modifying things, you only need to run `source\common\doc` again to update docs.