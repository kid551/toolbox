In order to debug the SAFS Reference Generation locally, you need to:

1. Delete the pushing parts of `update.safs.reference` in safs.build.xml, which has been done in this folder.
2. As we use the local repository of SAFS project instead of downloading from GitHub, we need to rename the 'Core' as 'safsdev.core', 'build' as 'safsdev.build', 'SAFS-Android-Remote-Control' as 'droid.remotecontrol', 'SAFS-Android-Engine' as 'droid.engine', 'SAFS-Android-Messenger' as 'droid.messenger'.