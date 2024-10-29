import matplotlib.pyplot as plt
import numpy as np
import sys
import os

sys.path.append('//Users//jamesswift//Documents//git//BCI2kReader')

# Get the directory of script
scriptdir = os.path.dirname(__file__)

datafile = scriptdir+'//TestData//ECOGS001R01.dat'
chToPlot = 0

from BCI2kReader import BCI2kReader as b2k

# Load BCI2000 *.dat file
with b2k.BCI2kReader(datafile) as BCI2000Data: # opens a stream to the dat file
    
    signal, states=BCI2000Data.readall()    # get data

# Make variables easy to read
sr           = BCI2000Data.samplingrate
time         = np.arange(0,signal[chToPlot].size/sr,1/sr) 
signaltoplot = signal[chToPlot]                           
stimcode     = states['StimulusCode'][0]                 

# Plot example signal
fig1, ax1 = plt.subplots()
ax1.plot(time, signaltoplot)
plt.xlabel('Time (s)')
plt.ylabel('Voltage (uV)')
plt.title('EEG Ch %d' % chToPlot)

# Print example parameter
print('Sampling Rate: %dHz' % sr) # print sampling rate

# plot stimulus code
fig2, ax2 = plt.subplots()
ax2.plot(time, stimcode)
plt.xlabel('Time (s)')
plt.ylabel('Stimulus Code')
plt.title('Stimulus Code')

plt.show()
