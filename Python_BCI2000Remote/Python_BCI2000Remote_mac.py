## import important libraries
import random, sys, os

## import some libraries from PsychoPy
from psychopy import visual, core, event, prefs 

## Get user input on bci2000 prog directory
default     = '/Applications/BCI2000/trunk/prog'
BCI2000prog = input('Enter path to BCI2000 prog directory (default is '+default+'): ')
if not BCI2000prog:
   BCI2000prog = default
   
## Set BCI2000 path
sys.path.append(BCI2000prog)

## BCI2000 setup
import BCI2000Remote
bci = BCI2000Remote.BCI2000Remote()
bci.WindowVisible = True
bci.WindowTitle   = 'Python Application Demo'
bci.SubjectID     = 'TestSubject'

bci.Connect()
bci.Execute('cd ${BCI2000LAUNCHDIR}')

# Set log level to 0
bci.Execute('Set LogLevel 0')

# Create new event to capture stimulus code (must be done before startup)
# Events are set asynchronously from block time, at the current sample
bci.Execute('ADD EVENT Square 1 0')

## Choose modules to startup
bci.StartupModules(('SignalGenerator --LogKeyboard=1', 'DummySignalprocessing', 'DummyApplication'))

bci.Execute('Wait for Connected')

## Load parameters
bci.LoadParametersRemote('../parms/fragments/amplifiers/SignalGenerator.prm')

## Visualize States
bci.Execute('visualize watch KeyDown')
bci.Execute('visualize watch Square')
bci.Execute('visualize watch SourceTime')

## Application setup
#create a window
mywin = visual.Window([800,600],monitor="testMonitor", units="deg", color="black")
#mywin = visual.Window(monitor="testMonitor", fullscr=1, screen=1, units="deg", color="black")

#initialize rectangle
rect = visual.Rect(
    win=mywin,
    units="pix",
    pos=(-800/2+400, -600/2+300),
    #pos=(-1920/2+200, -1080/2+200),
    width=300,
    height=300,
    fillColor="black",
    lineColor="black"
    )


# initialize instructions 
text = visual.TextStim(
    win=mywin,
    text="Press any key to start. Press and hold to stop",
    color="white"
)

# Wait for Running
bci.Execute('Wait for Running')


## Start application
text.draw()
mywin.flip()
event.waitKeys()
mywin.flip()

core.wait(1)

for i in range(10):

    # Allow for exit on key press
    if len(event.getKeys())>0:
        print(len(event.getKeys()))
        bci.Execute('SET STATE Running 0') 
        #mywin.close()
        #core.quit()
        sys.exit(0)
        break
    
    event.clearEvents()
    
    # make screen white
    rect.fillColor="white"
    rect.draw()
    mywin.flip()
    
    # Set bci2000 event to 1
    bci.Execute('Set event Square 1')
    
    ## Hold for 1 s
    core.wait(1)
    
    # make screen black
    rect.fillColor="black"
    rect.draw()
    mywin.flip()
    
    # set bci2000 event to 0
    bci.Execute('Set event Square 0')
    
    ## Hold for 1 s
    core.wait(1)
    

## cleanup
bci.Execute('SET STATE Running 0')
#mywin.close()
#core.quit()
sys.exit(0)