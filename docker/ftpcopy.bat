docker save webappmvc:latest -o image.tar
docker save consoleapp:latest -o consoleappimage.tar
psftp aurel@ftm.collectioninventory.com -pw Auka192Smd192 -b ftpscript.scr
psftp aurel@ftm.collectioninventory.com -pw Auka192Smd192 -b ftpscript2.scr
putty -ssh ftm.collectioninventory.com -l root -pw Auka192Smd192 -m "remotecmd.txt" -t

C:
cd C:\Program Files (x86)\Google\Chrome\Application
chrome.exe http://ftm.collectioninventory.com
pause
