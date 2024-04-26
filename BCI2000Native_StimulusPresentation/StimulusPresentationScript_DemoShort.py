import sys
import os
import random

default = 'C:\\bci2000.x64\\tools\\python\\'
BCI2000_python_tools = input('Enter path to BCI2000 python tools (default is '+default+'): ')
if not BCI2000_python_tools:
   BCI2000_python_tools = default

sys.path.append(BCI2000_python_tools)

from convert_bciprm import *

def float_range( start, stop, step ):
    l = []
    i = start
    q = 0
    while i < stop:
        l.append( i )
        q += 1
        i = start + q * step
    return l

print( "----Loading Settings----" )

settings = BCISettings() 

settings[ "BCI2000Path" ] = os.path.join( BCI2000_python_tools, "..", ".." )
print( "BCI2000Path: ", settings.BCI2000Path )


settings[ "TaskDuration         " ] = '2s'
settings[ "ISIDuration          " ] = '1.5s'
settings[ "InstructionDuration  " ] = '30s'
settings[ "CaptionSwitch        " ] = '1'
settings[ "parm_filename        " ] = os.path.join( settings[ "BCI2000Path" ], "parms", "demo_parms_py.prm" )

settings.InstructCaption   = [ 'Stimulus Presentation Task. Press space to continue', 'End of task.' ]


print( "----Done Loading Settings----" )

print( "----Reading Default Params----" )

param = read_bciprm( os.path.join( BCI2000_python_tools, "default.prm" ) )

print( "----Done Reading Params----" )

print( "----Setting Params----" )

n_stimuli = 62
n_rows = 7

param[ 'Application' ]['Stimuli']['Stimuli']['Section        '] = 'Application'
param[ 'Application' ]['Stimuli']['Stimuli']['Type           '] = 'matrix'
param[ 'Application' ]['Stimuli']['Stimuli']['DefaultValue   '] = ''
param[ 'Application' ]['Stimuli']['Stimuli']['LowRange       '] = ''
param[ 'Application' ]['Stimuli']['Stimuli']['HighRange      '] = ''
param[ 'Application' ]['Stimuli']['Stimuli']['Comment        '] = 'captions and icons to be displayed, sounds to be played for different stimuli'
param[ 'Application' ]['Stimuli']['Stimuli']['Value          '] = bcimatrix( n_rows, n_stimuli )
param[ 'Application' ]['Stimuli']['Stimuli']['RowLabels      '] = bcilist( n_rows )
param[ 'Application' ]['Stimuli']['Stimuli']['ColumnLabels   '] = bcilist( n_stimuli )

param.Application.Stimuli.Stimuli.RowLabels[0]  = 'caption'
param.Application.Stimuli.Stimuli.RowLabels[1]  = 'icon'
param.Application.Stimuli.Stimuli.RowLabels[2]  = 'audio'
param.Application.Stimuli.Stimuli.RowLabels[3]  = 'StimulusDuration'
param.Application.Stimuli.Stimuli.RowLabels[4]  = 'AudioVolume'
param.Application.Stimuli.Stimuli.RowLabels[5]  = 'Category'
param.Application.Stimuli.Stimuli.RowLabels[6]  = 'EarlyOffsetExpression'

directory   = os.path.join( settings[ "BCI2000Path" ], "prog", "images" )
task_images = os.listdir( directory )
task_images = [f for f in task_images if os.path.isfile(directory+'/'+f)]

for i in task_images:
    if not i.endswith('.bmp'):
        task_images.remove(i)

random.shuffle( task_images )

# Study images 1-50
for idx in range( 0, len( task_images ) ):
    param.Application.Stimuli.Stimuli.ColumnLabels[ idx ]    = str( idx + 1 )
    param.Application.Stimuli.Stimuli.Value[ 0 ][ idx ]      = ''
    param.Application.Stimuli.Stimuli.Value[ 1 ][ idx ]      = os.path.join( "..", "prog", "images", task_images[ idx ] )
    param.Application.Stimuli.Stimuli.Value[ 2 ][ idx ]      = ''
    param.Application.Stimuli.Stimuli.Value[ 3 ][ idx ]      = settings.TaskDuration
    param.Application.Stimuli.Stimuli.Value[ 4 ][ idx ]      = '0'      
    param.Application.Stimuli.Stimuli.Value[ 5 ][ idx ]      = 'image' 
    param.Application.Stimuli.Stimuli.Value[ 6 ][ idx ]      = '' 


# inter-stimulus interval (fixation cross) 51
idx=50
param.Application.Stimuli.Stimuli.ColumnLabels[ idx ]    = str( idx + 1 )
param.Application.Stimuli.Stimuli.Value[ 0 ][ idx ]      = '+'
param.Application.Stimuli.Stimuli.Value[ 1 ][ idx ]      = ''
param.Application.Stimuli.Stimuli.Value[ 2 ][ idx ]      = ''
param.Application.Stimuli.Stimuli.Value[ 3 ][ idx ]      = str( settings.ISIDuration )
param.Application.Stimuli.Stimuli.Value[ 4 ][ idx ]      = '0' 
param.Application.Stimuli.Stimuli.Value[ 5 ][ idx ]      = 'fixation'
param.Application.Stimuli.Stimuli.Value[ 6 ][ idx ]      = ''


# Instructions 61-62
idx_iter = 0
for idx in range( 60, 60 + len( settings.InstructCaption ) ):
    param.Application.Stimuli.Stimuli.ColumnLabels[ idx ] = str( idx )
    param.Application.Stimuli.Stimuli.Value[ 0 ][ idx ]      = settings.InstructCaption[ idx_iter ]
    param.Application.Stimuli.Stimuli.Value[ 1 ][ idx ]      = ''
    param.Application.Stimuli.Stimuli.Value[ 2 ][ idx ]      = ''
    param.Application.Stimuli.Stimuli.Value[ 3 ][ idx ]      = settings.InstructionDuration
    param.Application.Stimuli.Stimuli.Value[ 4 ][ idx ]      = '0'    
    param.Application.Stimuli.Stimuli.Value[ 5 ][ idx ]      = 'instruction' 
    param.Application.Stimuli.Stimuli.Value[ 6 ][ idx ]      = 'KeyDown == 32' # space key 
    
    idx_iter = idx_iter + 1

# Sequence
taskseq = []
for img in range( 1, len(task_images) + 1 ):
    taskseq += [ 51, img ]

seq = [ 61, *taskseq, 62 ]

param.Application.Sequencing.Sequence.Section      = 'Application'
param.Application.Sequencing.Sequence.Type         = 'intlist'
param.Application.Sequencing.Sequence.DefaultValue = '1'
param.Application.Sequencing.Sequence.LowRange     = '1'
param.Application.Sequencing.Sequence.HighRange    = ''
param.Application.Sequencing.Sequence.Comment      = 'Stimuli sequence (deterministic mode)'
param.Application.Sequencing.Sequence.Value        = [ str( i ) for i in seq ]
param.Application.Sequencing.Sequence.NumericValue = seq 

#
param.Application.Sequencing.SequenceType.Section              = 'Application'
param.Application.Sequencing.SequenceType.Type                 = 'int'
param.Application.Sequencing.SequenceType.DefaultValue         = '0'
param.Application.Sequencing.SequenceType.LowRange             = '0'
param.Application.Sequencing.SequenceType.HighRange            = '1'
param.Application.Sequencing.SequenceType.Comment              = 'Sequence of stimuli is 0 deterministic, 1 random (enumeration)'
param.Application.Sequencing.SequenceType.Value                = '0'

#
param.Application.Stimuli.CaptionSwitch.Section       = 'Application'
param.Application.Stimuli.CaptionSwitch.Type          = 'int'
param.Application.Stimuli.CaptionSwitch.DefaultValue  = '1'
param.Application.Stimuli.CaptionSwitch.LowRange      = '0'
param.Application.Stimuli.CaptionSwitch.HighRange     = '1'
param.Application.Stimuli.CaptionSwitch.Comment       = 'Present captions (boolean)'
param.Application.Stimuli.CaptionSwitch.Value         = [ settings.CaptionSwitch ]

#
param.Application.Stimuli.CaptionHeight.Section      = 'Application'
param.Application.Stimuli.CaptionHeight.Type         = 'int'
param.Application.Stimuli.CaptionHeight.DefaultValue = '0'
param.Application.Stimuli.CaptionHeight.LowRange     = '0'
param.Application.Stimuli.CaptionHeight.HighRange    = '100'
param.Application.Stimuli.CaptionHeight.Comment      = 'Height of stimulus caption text in percent of screen height'
param.Application.Stimuli.CaptionHeight.Value        = [ '5' ]

print( "----Done Setting Params----" )
print( "----Converting Params----" )

convert_bciprm( param, settings.parm_filename )

print( f"----Params Saved to { settings.parm_filename }----" )

