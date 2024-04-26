% BCI2000 Stimulus Presentation Demo Script
% 
% StimulusPresentationScript_Demo creates a parameter fragment that can be
% loaded into BCI2000 to create a stimulus presentation experiment.
% 
% This demo script will take the image files located in the BCI2000 prog
% directory and create a stimuli matrix containing these images, variable
% duration fixation cross stimuli, instructions, and a sync pulse. 
% 
% Change the n_rows and n_stimuli variables to store more information with
% the stimuli or add additional stimuli. Best practice is to separate
% stimuli into banks (e.g. 1-25, 101-125, etc) for easy evaluation later. 
% 
% Note that every stimulus needs to have an index for every row desired,
% even if that row label is not meaningful for the stimulus.
% 
% A sequence is created to alternate the fixation cross stimuli with the
% image stimuli.
% 
% The stimuli and meaningful parameters are written into a param
% variable and stored as a *.prm file using the convert_bciprm function.

%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
%% Author: James Swift <swift@neurotechcenter.org>
%%
%% $BEGIN_BCI2000_LICENSE$
%% 
%% This file is part of BCI2000, a platform for real-time bio-signal research.
%% [ Copyright (C) 2000-2021: BCI2000 team and many external contributors ]
%% 
%% BCI2000 is free software: you can redistribute it and/or modify it under the
%% terms of the GNU General Public License as published by the Free Software
%% Foundation, either version 3 of the License, or (at your option) any later
%% version.
%% 
%% BCI2000 is distributed in the hope that it will be useful, but
%%                         WITHOUT ANY WARRANTY
%% - without even the implied warranty of MERCHANTABILITY or FITNESS FOR
%% A PARTICULAR PURPOSE.  See the GNU General Public License for more details.
%% 
%% You should have received a copy of the GNU General Public License along with
%% this program.  If not, see <http://www.gnu.org/licenses/>.
%% 
%% $END_BCI2000_LICENSE$
%% http://www.bci2000.org 
%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%

%% Set the path of the BCI2000 main directory here
prompt               = {'Enter path to BCI2000 root directory: '};
dlgtitle             = 'Enter BCI2000 path';
fieldsize            = [1 45];
definput             = {fullfile('C:','bci2000.x64')};
userinput            = inputdlg(prompt,dlgtitle,fieldsize,definput);
settings.BCI2000path = userinput{1};

% Add BCI2000 tools to path
addpath(genpath(fullfile(settings.BCI2000path,'tools')))

%% Settings
settings.TaskDuration        = '2s';
settings.ISIDuration         = '1.5s';
settings.InstructionDuration = '30s';

settings.parm_filename       = fullfile(settings.BCI2000path,'parms','demo_parms.prm');

settings.InstructCaption     = {'Stimulus Presentation Task. Press space to continue'; 'End of task.'};

%% Get task images
task_images = dir(fullfile(settings.BCI2000path,'prog','images','*.bmp'));

%% Set up unique stimulus codes, separated into banks for easy evaluation
n_stimuli = 62; % Total events
n_rows    = 7;

% break down into blocks for easier analysis later
% 1-50:  image stimuli
% 51:    inter-stimulus interval (variable duration)
% 61-62: instructions

% Set up Stimuli
param.Stimuli.Section         = 'Application';
param.Stimuli.Type            = 'matrix';
param.Stimuli.DefaultValue    = '';
param.Stimuli.LowRange        = '';
param.Stimuli.HighRange       = '';
param.Stimuli.Comment         = 'captions and icons to be displayed';
param.Stimuli.Value           = cell(n_rows,n_stimuli);
param.Stimuli.Value(:)        = {''};
param.Stimuli.RowLabels       = cell(n_rows,1);
param.Stimuli.RowLabels(:)    = {''};
param.Stimuli.ColumnLabels    = cell(1,n_stimuli);
param.Stimuli.ColumnLabels(:) = {''};

param.Stimuli.RowLabels{1}  = 'caption';
param.Stimuli.RowLabels{2}  = 'icon';
param.Stimuli.RowLabels{3}  = 'audio';
param.Stimuli.RowLabels{4}  = 'StimulusDuration';
param.Stimuli.RowLabels{5}  = 'AudioVolume';
param.Stimuli.RowLabels{6}  = 'Category';
param.Stimuli.RowLabels{7}  = 'EarlyOffsetExpression';

%% Study images 1-50
for idx = 1:length(task_images)
    param.Stimuli.ColumnLabels{idx} = sprintf('%d',idx);
    param.Stimuli.Value{1,idx}      = '';
    param.Stimuli.Value{2,idx}      = sprintf('%s',...
                fullfile('..','prog','images',task_images(idx).name));
    param.Stimuli.Value{3,idx}      = '';
    param.Stimuli.Value{4,idx}      = settings.TaskDuration;
    param.Stimuli.Value{5,idx}      = '0';      
    param.Stimuli.Value{6,idx}      = 'image'; 
    param.Stimuli.Value{7,idx}      = ''; 
end 


%% inter-stimulus interval (fixation cross) 51
idx = 51;

param.Stimuli.ColumnLabels{idx} = sprintf('%d',idx);
param.Stimuli.Value{1,idx}      = '+';
param.Stimuli.Value{2,idx}      = '';
param.Stimuli.Value{3,idx}      = '';
param.Stimuli.Value{4,idx}      = num2str(settings.ISIDuration,7);
param.Stimuli.Value{5,idx}      = '0';      
param.Stimuli.Value{6,idx}      = 'fixation'; 
param.Stimuli.Value{7,idx}      = ''; 


%% Instructions 61-62
idx_iter = 1;
for idx = 61:60+length(settings.InstructCaption)
    param.Stimuli.ColumnLabels{idx} = sprintf('%d',idx);
    param.Stimuli.Value{1,idx}      = settings.InstructCaption{idx_iter};
    param.Stimuli.Value{2,idx}      = '';
    param.Stimuli.Value{3,idx}      = '';
    param.Stimuli.Value{4,idx}      = settings.InstructionDuration;
    param.Stimuli.Value{5,idx}      = '0';    
    param.Stimuli.Value{6,idx}      = 'instruction'; 
    param.Stimuli.Value{7,idx}      = 'KeyDown == 32'; % space key 
    
    idx_iter = idx_iter + 1;
end     


%% Sequence
% 1-50:    image stimuli
% 51:      inter-stimulus interval (variable duration)
% 61-62: instructions

randOrder = randperm(length(task_images));
taskseq   = [];
for i = 1:length(task_images)
    currentImage = randOrder(i);
    taskseq      = [taskseq 51 currentImage];
end

seq = [ 61 taskseq 62 ]';


param.Sequence.Section      = 'Application';
param.Sequence.Type         = 'intlist';
param.Sequence.DefaultValue = '1';
param.Sequence.LowRange     = '1';
param.Sequence.HighRange    = '';
param.Sequence.Comment      = 'Sequence in which stimuli are presented';
param.Sequence.Value        = cellfun(@num2str, num2cell(seq), 'un',0);
param.Sequence.NumericValue = seq;


%% Sequence Type
param.SequenceType.Section      = 'Application';
param.SequenceType.Type         = 'int';
param.SequenceType.DefaultValue = '0';
param.SequenceType.LowRange     = '0';
param.SequenceType.HighRange    = '1';
param.SequenceType.Comment      = '0 deterministic, 1 random (enumeration)';
param.SequenceType.Value        = {'0'};


%% Caption Switch
param.CaptionSwitch.Section      = 'Application';
param.CaptionSwitch.Type         = 'int';
param.CaptionSwitch.DefaultValue = '1';
param.CaptionSwitch.LowRange     = '0';
param.CaptionSwitch.HighRange    = '1';
param.CaptionSwitch.Comment      = 'Present captions (boolean)';
param.CaptionSwitch.Value        = {'1'};


%% write the param struct to a bci2000 parameter file
parameter_lines = convert_bciprm( param );
fid = fopen(settings.parm_filename, 'w');

for i=1:length(parameter_lines)
    fprintf( fid, '%s', parameter_lines{i} );
    fprintf( fid, '\r\n' );
end
fclose(fid);
