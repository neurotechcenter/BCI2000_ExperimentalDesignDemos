## import important libraries
import sys, random, os

## import some libraries from PsychoPy
from psychopy import core, visual, event, prefs 

numTrials = 10;
pressedKey = [];

## Get user input on bci2000 prog directory
default     = 'C:\\bci2000.x64\\prog'
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
# bci.Execute('Set LogLevel 0')

# Create new event to capture stimulus code (must be done before startup)
# Events are set asynchronously from block time, at the current sample
bci.Execute('ADD EVENT Square 2 0')

## Choose modules to startup
bci.StartupModules(('SignalGenerator --LogKeyboard=1', 'DummySignalprocessing', 'DummyApplication'))

bci.Execute('Wait for Connected')

## Load parameters
bci.LoadParametersRemote('..\\parms\\fragments\\amplifiers\\SignalGenerator.prm')

## Visualize States
bci.Execute('visualize watch KeyDown')
bci.Execute('visualize watch Square')
bci.Execute('visualize watch SourceTime')

## Wait for Running
bci.Execute('Wait for Running')


## Application setup
#create a window
mywin = visual.Window([800,600],monitor="testMonitor", units="deg", color="black")
#mywin = visual.Window(monitor="testMonitor", fullscr=1, screen=2, units="deg", color="black")

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
    text="Press left or right arrow key. Press space to start. Press q to quit.",
    color="white"
)

# initialize images
path_to_image_file_1 = os.path.join(BCI2000prog, "images", "1.bmp")
path_to_image_file_2 = os.path.join(BCI2000prog, "images", "2.bmp")


## Start application
text.draw()
mywin.flip()
event.waitKeys(float('inf'),'space')
mywin.flip()

core.wait(1)

for i in range(numTrials):

    event.clearEvents()
    
    ## Start next trial
    # Trial instructions
    text = visual.TextStim(
        win=mywin,
        text="Press left or right arrow key",
        color="white"
    )
    text.draw()
    mywin.flip()
    
    # set bci2000 event to 0
    bci.Execute('Set event Square 0')
    Square = bci.GetEventVariable('Square').value
    
    ST = bci.GetStateVariable('SourceTime').value
    print('SourceTime Value: ' + str(ST))
    
    ## Wait for key press
    pressedKey = event.waitKeys(float('inf'), ['left', 'right', 'q'])

    # present image 1
    if pressedKey == ['left']:
        image_stim = visual.ImageStim(mywin, image=path_to_image_file_1)
        image_stim.draw()
        mywin.flip()
        
        # Set bci2000 event to 1
        bci.Execute('Set event Square 1')
        Square = bci.GetEventVariable('Square').value
        
        # Display result for 1s
        core.wait(1)

    # present image 2
    elif pressedKey == ['right']:
        image_stim = visual.ImageStim(mywin, image=path_to_image_file_2)
        image_stim.draw()
        mywin.flip()
        
        # set bci2000 event to 2
        bci.Execute('Set event Square 2')
        Square = bci.GetEventVariable('Square').value
        
        # Display result for 1s
        core.wait(1)
        
    # Allow for exit on q key press
    elif pressedKey == ['q']:
        bci.Execute('SET STATE Running 0') 
        mywin.close()
        core.quit()
        

## cleanup
bci.Execute('SET STATE Running 0')
mywin.close()
core.quit()