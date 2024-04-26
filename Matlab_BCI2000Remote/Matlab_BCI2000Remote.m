%% BCI2000 Integration 
clear all;

%% Setup path and subject name
prompt       = {'Enter path to BCI2000 root directory: '};
dlgtitle     = 'Enter BCI2000 path';
fieldsize    = [1 45];
definput     = {fullfile('C:','bci2000.x64')};
userinput    = inputdlg(prompt,dlgtitle,fieldsize,definput);
BCI2000root  = userinput{1};

subject_name = 'matlabsub';

passSourceTime = 0.0;

%% Load BCI2000Remote libraries
if not(libisloaded('bci'))
    loadlibrary(fullfile(BCI2000root,'prog','BCI2000RemoteLib64'),...
        fullfile(BCI2000root,'src','core','Operator','BCI2000Remote','BCI2000RemoteLib.h'), 'alias', 'bci')
end 
libfunctions('bci')

%%
bciHandle = calllib('bci', 'BCI2000Remote_New');
calllib('bci', 'BCI2000Remote_SetOperatorPath', bciHandle, fullfile(BCI2000root,'prog','Operator'));

% if we fail to establish a connection to BCI2000Remote
if calllib('bci', 'BCI2000Remote_Connect', bciHandle) ~= 1
    fprintf('bci connect fail!')
    calllib('bci', 'BCI2000Remote_Delete', bciHandle); % call BCI2000Remote_Delete to recovery the memory
    return
end

%% Startup BCI2000
calllib('bci', 'BCI2000Remote_Execute', bciHandle,'Change directory $BCI2000LAUNCHDIR', 0);
calllib('bci', 'BCI2000Remote_Execute', bciHandle,'Show window; Set title ${Extract file base $0}', 0);
calllib('bci', 'BCI2000Remote_Execute', bciHandle,'Reset system', 0);

% Create new parameter (must be done before startup)
calllib('bci', 'BCI2000Remote_Execute', bciHandle, 'Add Parameter Application:TestParameterField string TestParameter= FirstNewParameter % % %', 0); 

% Define S tates
% Define new state with 2 bytes of information (2 colors)
calllib('bci', 'BCI2000Remote_Execute', bciHandle, 'add event Square 1 0', 0);

% Startup system localhost
calllib('bci', 'BCI2000Remote_Execute', bciHandle,'Startup system localhost', 0);

% Make BCI2000 visible
calllib('bci', 'BCI2000Remote_SetWindowVisible', bciHandle, 1);

% Set log level to 0
calllib('bci', 'BCI2000Remote_Execute', bciHandle, 'Set LogLevel 0', 0);

%% Establish connection to three modules
SourceModule = 'SignalGenerator';
modules      = libpointer('stringPtrPtr', {[SourceModule ' --local --LogKeyboard=1'], 'DummySignalProcessing', 'DummyApplication'});
calllib('bci', 'BCI2000Remote_StartupModules2', bciHandle, modules, 3);

% Wait for connected before loading parameters!
calllib('bci', 'BCI2000Remote_Execute', bciHandle, 'Wait for Connected', 0);

%% Parameters
% Load parameter files
calllib('bci', 'BCI2000Remote_LoadParametersRemote', bciHandle, fullfile(BCI2000root,'parms','fragments','amplifiers','SignalGenerator.prm'));
calllib('bci', 'BCI2000Remote_LoadParametersRemote', bciHandle, fullfile(BCI2000root,'parms','Source','transmit_channel_one.prm'));

% Set parameter values
calllib('bci', 'BCI2000Remote_SetDataDirectory', bciHandle, fullfile(BCI2000root,'data','BJH'));
calllib('bci', 'BCI2000Remote_SetSubjectID',     bciHandle, subject_name);

% Get parameter value
SamplingRate = calllib('bci', 'BCI2000Remote_GetParameter', bciHandle, 'SamplingRate');
fprintf(['\nSamplingRate: ' num2str(SamplingRate) 'Hz\n'])


%% Inspect states
%Set watches to appear automatically
calllib('bci', 'BCI2000Remote_Execute', bciHandle, 'visualize watch KeyDown',    0);
calllib('bci', 'BCI2000Remote_Execute', bciHandle, 'visualize watch Square',     0);
calllib('bci', 'BCI2000Remote_Execute', bciHandle, 'visualize watch SourceTime', 0);

% Get state value
% Immediately after data block has been acquired from hardware, the 
% DataIOFilter writes a 16-bit millisecond-resolution time stamp into the 
% SourceTime state. Block duration is measured as the difference between
% two consecutive time stamps.
[~,~,~,SourceTime]=calllib('bci', 'BCI2000Remote_GetStateVariable', bciHandle, 'SourceTime', passSourceTime);
fprintf(['\nSourceTime: ', num2str(SourceTime), '\n'])

%% Wait for BCI2000 to start running before starting application
calllib('bci', 'BCI2000Remote_Execute', bciHandle, 'Wait for Running', 0);


%% Dummy Psychtoolbox Application
screens      = Screen('Screens');
screenNumber = max(screens);

Screen('Preference', 'SkipSyncTests', 2); % skip psychtoolbox screen calibration

% Define black, white, and grey
black = 1;
white = WhiteIndex(screenNumber);
grey  = white / 2;  

% Open an on screen window
% [window, windowRect] = PsychImaging('OpenWindow', screenNumber, grey); 
[window, windowRect] = PsychImaging('OpenWindow', screenNumber, grey, [0 0 600 600]); % [left, top, right, bottom]


% Alternate a black and white square three times
for i = 1:10
    % Hold any key to quit
    KeyPressed = KbCheck;
    if KeyPressed
        sca; % screen close all

        % Stop running BCI2000
        calllib('bci', 'BCI2000Remote_SetStateVariable', bciHandle, 'Running', 0);
        
        return;
    end
    
    % windowID, color, [left, top, right, bottom]
    Screen('FillRect', window, white, [windowRect(3)/4 windowRect(4)/4 3*windowRect(3)/4 3*windowRect(4)/4]);
    Screen('Flip', window); % flip front and back display surfaces
    
    % BCI2000 set white rectangle to 1 (true)
    calllib('bci', 'BCI2000Remote_Execute', bciHandle, 'Set event Square 1', 0);

    WaitSecs(1);

    Screen('FillRect', window, black, [windowRect(3)/4 windowRect(4)/4 3*windowRect(3)/4 3*windowRect(4)/4]);
    Screen('Flip', window); % flip front and back display surfaces
    
    % BCI2000 set white rectangle to 0 (false)
    calllib('bci', 'BCI2000Remote_Execute', bciHandle, 'Set event Square 0', 0);

    WaitSecs(1);
    
%     % Get state value
    [~,~,~,SourceTime] = calllib('bci', 'BCI2000Remote_GetStateVariable', bciHandle, 'SourceTime', passSourceTime);
    fprintf(['\nSourceTime: ', num2str(SourceTime), '\n'])
end

KbWait; % wait for key press
sca;    % screen close all
 
% Stop running BCI2000
calllib('bci', 'BCI2000Remote_SetStateVariable', bciHandle, 'Running', 0);




    
    
    