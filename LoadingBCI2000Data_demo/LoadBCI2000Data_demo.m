clear;
close all;

%% Add BCI2000 tools to path
addpath(genpath('C:/BCI2000.x64/tools/mex'))

%% Load BCI2000 *.dat file
filename = 'TestData/ECOGS001R01.dat';
[signal, states, parameters] = load_bcidat(filename);

%% Make variables easy to read
signal       = double(signal);                  % signal
stimCode     = states.StimulusCode;             % stimulus code
SamplingRate = parameters.SamplingRate.NumericValue; % sampling rate

% create time vector to convert x-axis to seconds
t = 1/SamplingRate:1/SamplingRate:size(signal,1)/SamplingRate; % time in seconds

%% Plot example signal
chToPlot     = 1;                  % channel to plot
signalToPlot = signal(:,chToPlot); % signal of channel to plot

% plot EEG signal
figure;
plot(t,signalToPlot)
xlabel('Time (s)')
ylabel('Voltage (uV)')
title(['EEG Ch ', num2str(chToPlot)])

%% Print example parameter
% print sampling rate
fprintf(['Sampling Rate: ', num2str(SamplingRate), 'Hz\n'])

%% Plot example state
% plot stimulus code
figure;
plot(t,stimCode)
xlabel('Time (s)')
ylabel('Stimulus Code')
title('Stimulus Code')
