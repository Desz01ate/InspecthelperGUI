#header "please do a 'pip install psutil' as it should solve most problem"
try:
	import os.path,os,sys,time
	import psutil
	import thread
	from gtts import gTTS #google text-to-speech api
	import playsound #mp3 player
except:
	os.system("color 04") #RED
	for i in range(5):
		print("Some modules are missing. Please restart inspecthelper after the modules installation")
		time.sleep(0.5)
	os.system("color 07") #DEFAULT 
	os.system("C:\Python27\Scripts\pip.exe install psutil")
	os.system("C:\Python27\Scripts\pip.exe install gtts")
	os.system("C:\Python27\Scripts\pip.exe install playsound")
	


#constant
idle = 64
belownormal=16384
normal=32
abovenormal=32768
high=128
realtime=256
currPath = os.getcwd()
##EXPERIMENT##
_currPath = os.path.realpath(__file__)
##EXPERIMENT##
delay = 10 #measure in second, not recommend to do very frequence as it gonna make your cpu work so hard
High_Performance = "8c5e7fda-e8bf-4a96-9a85-a6e23a8c635c"
Balanced = "381b4222-f694-41f0-9685-ff5bb260df2e"
counter = 0



#flag variable
history = "null"
mp3filelist = []

#function
def getFileName(name):
	return _currPath.replace("inspecthelper.py",name)

def variableName(var,namespace):
	return [name for name in namespace if namespace[name] is var]

def isExist(filename):
	return os.path.isfile(filename)

def fileLoader(path):
	rList = {}
	with open(path,"r") as f:
		for line in f:
			if "#" not in line:
				splitLine = line.split(",")
				splitLine[1] = splitLine[1].replace("\n","")
				rList[str(splitLine[0])] = splitLine[1]
	return rList

def switchState(modestr):
	#os.system("powercfg /setactive %s"%mode)
	global history,counter
	modestrspf = modestr[1]
	modestrspf = modestrspf[:len(modestrspf)-4]
	if(history!=modestrspf):
		history=modestrspf
		tempfname = modestr[0]+".mp3"
		if isExist(getFileName(tempfname))==False:
			tts = gTTS(text="Currently on {0}".format(modestr[0]),lang='en')	
			tts.save(getFileName(tempfname))
		thread.start_new_thread(playsound.playsound,(getFileName(tempfname),)) #play sound in threading manner
		os.system('powercfg /setactive {0}'.format(High_Performance if modestrspf!="Default" else Balanced)) #High Performance Power Plan
		os.system('wmic process where name="{0}.exe" CALL setpriority {1} > null'.format(history,high)) #high priority for the process
		os.system('"{0}.bat"'.format(modestr[0])) #custom bat file for nvidiainspector config
		mp3filelist.append(tempfname)
	os.system("cls")
	return "\rCurrently on {0}".format(modestr[0])

def Core(state):
	SettingProc = fileLoader(getFileName("inspecthelper.settings"))
	while(state):
		procsList = []
		sender = ("Desktop","Default.exe")	
		for proc in psutil.process_iter():
			pvalue = "".join(proc.as_dict(attrs=['name']).values())
			procsList.append(pvalue)
		#for p in procsList:
			#for ps in SettingProc.items():
		for ps in SettingProc.items(): #this should help improve overall performance
			for p in procsList:
				if p == ps[1]: #the value of dict
					sender = ps #now the ps is not come as dict, it's a tuple
					break
			else:
				continue
			break
		print switchState(sender),
		time.sleep(delay)

#main flow
os.system("cls")
os.system("title Nvidia Inspector Profile Helper (x86) [Press Ctrl+C for option]")
try:
	#if isExist(currPath+"\%s"%filename):
	if isExist(getFileName("inspecthelper.settings")):
		os.system('wmic process where name="python.exe" CALL setpriority {0} > null'.format(belownormal))
		os.system('start {0}\ideafan\ideafan.exe'.format(_currPath.replace("\inspecthelper.py","")))
		Core(True)
	else:
		answer = raw_input("Setting file is not exist, would you like to create now? (Y/N) : ")
		if answer == "Y":
			file = open(filename,'w')
			file.write("### ONLY PROCESSES NAME ARE GIVEN BELOW THIS LINE IN FORM OF NAME,PROCESS NAME.EXE , ANY LINE WITH SHARP(#) WOULD BE IGNORED ###\nDummy,Dummy.exe\n")
			file.close()
			os.system("notepad %s"%filename)
		else:
			exit()
except KeyboardInterrupt:
	answer = str(raw_input("\nWhat do you want to do :\n1.) Open setting\n2.) Exit\nGoing for : "),)
	if answer == "1":
		os.system("notepad %s"%filename)
	else:
		switchState(("Default","Default.exe"))
		print("\nPROCESS SCANNER TERMINATED")
		sys.exit(0)
except Exception as e:
	print(e.message)

