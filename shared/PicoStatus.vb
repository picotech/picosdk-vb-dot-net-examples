'========================================================================================
'	Filename:			PicoStatus.vb
'
'	Description: 
'	This file defines the  status codes returned by a Pico device, a PC Oscilloscope 
'   or data logger and is based on the PicoStatus.h header file.
'
'   In comments, "<API>" is a placeholder for the name of the scope or
'   data logger API. For example, for the ps5000a API, it stands for
'   "PS5000A" Or "ps5000a".
'
'   Copyright (C) 2016 - 2017 Pico Technology Ltd. All rights reserved.
'
'========================================================================================

Module PicoStatus

    ' The PicoScope is functioning correctly.
    Public Const PICO_OK = &H0

    ' An attempt has been made to open more than <API>_MAX_UNITS.
    Public Const PICO_MAX_UNITS_OPENED = &H1

    ' Not enough memory could be allocated on the host machine.
    Public Const PICO_MEMORY_FAIL = &H2

    ' No Pico Technology device could be found.
    Public Const PICO_NOT_FOUND = &H3

    ' Unable to download firmware.
    Public Const PICO_FW_FAIL = &H4

    ' The driver Is busy opening a device.
    Public Const PICO_OPEN_OPERATION_IN_PROGRESS = &H5

    ' An unspecified failure occurred.
    Public Const PICO_OPERATION_FAILED = &H6

    ' The PicoScope Is Not responding to commands from the PC.
    Public Const PICO_NOT_RESPONDING = &H7

    ' The configuration information in the PicoScope Is corrupt Or missing.
    Public Const PICO_CONFIG_FAIL = &H8

    ' The picopp.sys file Is too old to be used with the device driver.
    Public Const PICO_KERNEL_DRIVER_TOO_OLD = &H9

    ' The EEPROM has become corrupt, so the device will use a default setting.
    Public Const PICO_EEPROM_CORRUPT = &HA

    ' The operating system on the PC Is Not supported by this driver.
    Public Const PICO_OS_NOT_SUPPORTED = &HB

    ' There is no device with the handle value passed.
    Public Const PICO_INVALID_HANDLE = &HC

    ' A parameter value is not valid.
    Public Const PICO_INVALID_PARAMETER = &HD

    ' The timebase is not supported or is invalid.
    Public Const PICO_INVALID_TIMEBASE = &HE

    ' The voltage range Is Not supported Or Is invalid.
    Public Const PICO_INVALID_VOLTAGE_RANGE = &HF

    ' The channel number Is Not valid on this device Or no channels have been set.
    Public Const PICO_INVALID_CHANNEL = &H10

    ' The channel set for a trigger Is Not available on this device.
    Public Const PICO_INVALID_TRIGGER_CHANNEL = &H11

    ' The channel set for a condition Is Not available on this device.
    Public Const PICO_INVALID_CONDITION_CHANNEL = &H12

    ' The device does Not have a signal generator.
    Public Const PICO_NO_SIGNAL_GENERATOR = &H13

    ' Streaming has failed to start Or has stopped without user request.
    Public Const PICO_STREAMING_FAILED = &H14

    ' Block failed to start - a parameter may have been set wrongly.
    Public Const PICO_BLOCK_MODE_FAILED = &H15

    ' A parameter that was required Is NULL.
    Public Const PICO_NULL_PARAMETER = &H16

    ' The current functionality Is Not available while using ETS capture mode.
    Public Const PICO_ETS_MODE_SET = &H17

    ' No data Is available from a run block call.
    Public Const PICO_DATA_NOT_AVAILABLE = &H18

    ' The buffer passed for the information was too small.
    Public Const PICO_STRING_BUFFER_TO_SMALL = &H19

    ' ETS Is Not supported on this device.
    Public Const PICO_ETS_NOT_SUPPORTED = &H1A

    ' The auto trigger time Is less than the time it will take to collect the pre-trigger data.
    Public Const PICO_AUTO_TRIGGER_TIME_TO_SHORT = &H1B

    ' The collection of data has stalled as unread data would be overwritten.
    Public Const PICO_BUFFER_STALL = &H1C

    ' Number of samples requested Is more than available in the current memory segment.
    Public Const PICO_TOO_MANY_SAMPLES = &H1D

    ' Not possible to create number of segments requested.
    Public Const PICO_TOO_MANY_SEGMENTS = &H1E

    ' A null pointer has been passed in the trigger function Or one of the parameters Is out of range.
    Public Const PICO_PULSE_WIDTH_QUALIFIER = &H1F

    ' One Or more of the hold-off parameters are out of range.
    Public Const PICO_DELAY = &H20

    ' One Or more of the source details are incorrect.
    Public Const PICO_SOURCE_DETAILS = &H21

    ' One Or more of the conditions are incorrect.
    Public Const PICO_CONDITIONS = &H22

    ' The driver's thread is currently in the <API>Ready callback 
    ' function And therefore the action cannot be carried out.
    Public Const PICO_USER_CALLBACK = &H23

    ' An attempt Is being made to get stored data while streaming. Either stop
    ' streaming by calling <API>Stop, Or use <API>GetStreamingLatestValues.
    Public Const PICO_DEVICE_SAMPLING = &H24

    ' Data Is unavailable because a run has Not been completed.
    Public Const PICO_NO_SAMPLES_AVAILABLE = &H25

    ' The memory segment index Is out of range.
    Public Const PICO_SEGMENT_OUT_OF_RANGE = &H26

    ' The device is busy so data cannot be returned yet.
    Public Const PICO_BUSY = &H27

    ' The start time to get stored data Is out of range.
    Public Const PICO_STARTINDEX_INVALID = &H28

    ' The information number requested Is Not a valid number.
    Public Const PICO_INVALID_INFO = &H29

    ' The handle Is invalid so no information Is available about the device. 
    ' Only PICO_DRIVER_VERSION Is available.
    Public Const PICO_INFO_UNAVAILABLE = &H2A

    ' The sample interval selected for streaming Is out of range.
    Public Const PICO_INVALID_SAMPLE_INTERVAL = &H2B

    ' ETS Is set but no trigger has been set. A trigger setting Is required for ETS.
    Public Const PICO_TRIGGER_ERROR = &H2C

    ' Driver cannot allocate memory.
    Public Const PICO_MEMORY = &H2D

    ' Incorrect parameter passed to the signal generator.
    Public Const PICO_SIG_GEN_PARAM = &H2E

    ' Conflict between the shots And sweeps parameters sent to the signal generator.
    Public Const PICO_SHOTS_SWEEPS_WARNING = &H2F

    ' A software trigger has been sent but the trigger source Is Not a software trigger.
    Public Const PICO_SIGGEN_TRIGGER_SOURCE = &H30

    ' An <API>SetTrigger call has found a conflict between the trigger source And the AUX output enable.
    Public Const PICO_AUX_OUTPUT_CONFLICT = &H31

    ' ETS mode Is being used And AUX Is set as an input.
    Public Const PICO_AUX_OUTPUT_ETS_CONFLICT = &H32

    ' Attempt to set different EXT input thresholds set for signal generator And oscilloscope trigger.
    Public Const PICO_WARNING_EXT_THRESHOLD_CONFLICT = &H33

    ' An <API>SetTrigger... function has set AUX as an output And the signal generator Is using it as a trigger.
    Public Const PICO_WARNING_AUX_OUTPUT_CONFLICT = &H34

    ' The combined peak to peak voltage And the analog offset voltage exceed the maximum voltage the signal generator can produce.
    Public Const PICO_SIGGEN_OUTPUT_OVER_VOLTAGE = &H35

    ' NULL pointer passed as delay parameter.
    Public Const PICO_DELAY_NULL = &H36

    ' The buffers for overview data have Not been set while streaming.
    Public Const PICO_INVALID_BUFFER = &H37

    ' The analog offset voltage Is out of range.
    Public Const PICO_SIGGEN_OFFSET_VOLTAGE = &H38

    ' The analog peak-to-peak voltage Is out of range.
    Public Const PICO_SIGGEN_PK_TO_PK = &H39

    ' A block collection has been cancelled.
    Public Const PICO_CANCELLED = &H3A

    ' The segment index Is Not currently being used.
    Public Const PICO_SEGMENT_NOT_USED = &H3B

    ' The wrong GetValues function has been called for the collection mode in use.
    Public Const PICO_INVALID_CALL = &H3C

    Public Const PICO_GET_VALUES_INTERRUPTED = &H3D

    ' The function Is Not available.
    Public Const PICO_NOT_USED = &H3F

    ' The aggregation ratio requested Is out of range.
    Public Const PICO_INVALID_SAMPLERATIO = &H40

    ' Device Is in an invalid state.
    Public Const PICO_INVALID_STATE = &H41

    ' The number of segments allocated Is fewer than the number of captures requested.
    Public Const PICO_NOT_ENOUGH_SEGMENTS = &H42

    ' A driver function has already been called And Not yet finished.
    ' Only one call to the driver can be made at any one time.
    Public Const PICO_DRIVER_FUNCTION = &H43

    ' Not used
    Public Const PICO_RESERVED = &H44

    ' An invalid coupling type was specified in <API>SetChannel.
    Public Const PICO_INVALID_COUPLING = &H45

    ' An attempt was made to get data before a data buffer was defined.
    Public Const PICO_BUFFERS_NOT_SET = &H46

    ' The selected downsampling mode (used for data reduction) Is Not allowed.
    Public Const PICO_RATIO_MODE_NOT_SUPPORTED = &H47

    ' Aggregation was requested in rapid block mode.
    Public Const PICO_RAPID_NOT_SUPPORT_AGGREGATION = &H48

    ' An invalid parameter was passed to <API>SetTriggerChannelProperties.
    Public Const PICO_INVALID_TRIGGER_PROPERTY = &H49

    ' The driver was unable to contact the oscilloscope.
    Public Const PICO_INTERFACE_NOT_CONNECTED = &H4A

    ' Resistance-measuring mode Is Not allowed in conjunction with the specified probe.
    Public Const PICO_RESISTANCE_AND_PROBE_NOT_ALLOWED = &H4B

    ' The device was unexpectedly powered down.
    Public Const PICO_POWER_FAILED = &H4C

    ' A problem occurred in <API>SetSigGenBuiltIn Or <API>SetSigGenArbitrary.
    Public Const PICO_SIGGEN_WAVEFORM_SETUP_FAILED = &H4D

    ' FPGA Not successfully set up.
    Public Const PICO_FPGA_FAIL = &H4E

    Public Const PICO_POWER_MANAGER = &H4F

    ' An impossible analog offset value was specified in <API>SetChannel.
    Public Const PICO_INVALID_ANALOGUE_OFFSET = &H50

    ' There Is an error within the device hardware.
    Public Const PICO_PLL_LOCK_FAILED = &H51

    ' There Is an error within the device hardware.
    Public Const PICO_ANALOG_BOARD = &H52

    ' Unable to configure the signal generator.
    Public Const PICO_CONFIG_FAIL_AWG = &H53

    ' The FPGA cannot be initialized, so unit cannot be opened.
    Public Const PICO_INITIALISE_FPGA = &H54

    ' The frequency for the external clock Is Not within 15% of the nominal value.
    Public Const PICO_EXTERNAL_FREQUENCY_INVALID = &H56

    ' The FPGA could Not lock the clock signal.
    Public Const PICO_CLOCK_CHANGE_ERROR = &H57

    ' You are trying to configure the AUX input as both a trigger And a reference clock.
    Public Const PICO_TRIGGER_AND_EXTERNAL_CLOCK_CLASH = &H58

    ' You are trying to congfigure the AUX input as both a pulse width qualifier And a reference clock.
    Public Const PICO_PWQ_AND_EXTERNAL_CLOCK_CLASH = &H59

    ' The requested scaling file cannot be opened.
    Public Const PICO_UNABLE_TO_OPEN_SCALING_FILE = &H5A

    ' The frequency of the memory Is reporting incorrectly.
    Public Const PICO_MEMORY_CLOCK_FREQUENCY = &H5B

    ' The I2C that Is being actioned Is Not responding to requests.
    Public Const PICO_I2C_NOT_RESPONDING = &H5C

    ' There are no captures available And therefore no data can be returned.
    Public Const PICO_NO_CAPTURES_AVAILABLE = &H5D

    ' The number of trigger channels Is greater than 4,
    ' except for a PS4824 where 8 channels are allowed for rising/falling/rising_or_falling trigger directions.
    Public Const PICO_TOO_MANY_TRIGGER_CHANNELS_IN_USE = &H5F

    ' When more than 4 trigger channels are set on a PS4824 And the direction Is out of range.
    Public Const PICO_INVALID_TRIGGER_DIRECTION = &H60

    '  When more than 4 trigger channels are set And their trigger condition states are Not <API>_CONDITION_TRUE.
    Public Const PICO_INVALID_TRIGGER_STATES = &H61

    ' The capture mode the device Is currently running in does Not support the current request.
    Public Const PICO_NOT_USED_IN_THIS_CAPTURE_MODE = &H5E

    Public Const PICO_GET_DATA_ACTIVE = &H103

    ' Codes 104 to 10B are used by the PT104 (USB) when connected via the Network Socket.

    ' The device Is currently connected via the IP Network socket And thus the call made Is Not supported.
    Public Const PICO_IP_NETWORKED = &H104

    ' An incorrect IP address has been passed to the driver.
    Public Const PICO_INVALID_IP_ADDRESS = &H105

    ' The IP socket has failed.
    Public Const PICO_IPSOCKET_FAILED = &H106

    ' The IP socket has timed out.
    Public Const PICO_IPSOCKET_TIMEDOUT = &H107

    ' Failed to apply the requested settings.
    Public Const PICO_SETTINGS_FAILED = &H108

    ' The network connection has failed.
    Public Const PICO_NETWORK_FAILED = &H109

    ' Unable to load the WS2 DLL.
    Public Const PICO_WS2_32_DLL_NOT_LOADED = &H10A

    ' The specified IP port Is invalid.
    Public Const PICO_INVALID_IP_PORT = &H10B

    ' The type of coupling requested Is Not supported on the opened device.
    Public Const PICO_COUPLING_NOT_SUPPORTED = &H10C

    ' Bandwidth limiting Is Not supported on the opened device.
    Public Const PICO_BANDWIDTH_NOT_SUPPORTED = &H10D

    ' The value requested for the bandwidth limit Is out of range.
    Public Const PICO_INVALID_BANDWIDTH = &H10E

    ' The arbitrary waveform generator Is Not supported by the opened device.
    Public Const PICO_AWG_NOT_SUPPORTED = &H10F

    ' Data has been requested with ETS mode set but run block has Not been called, 
    ' Or stop has been called.
    Public Const PICO_ETS_NOT_RUNNING = &H110

    ' White noise output Is Not supported on the opened device.
    Public Const PICO_SIG_GEN_WHITENOISE_NOT_SUPPORTED = &H111

    ' The wave type requested Is Not supported by the opened device.
    Public Const PICO_SIG_GEN_WAVETYPE_NOT_SUPPORTED = &H112

    ' The requested digital port number Is out of range (MSOs only).
    Public Const PICO_INVALID_DIGITAL_PORT = &H113

    ' The digital channel Is Not in the range <API>_DIGITAL_CHANNEL0 to
    ' <API>_DIGITAL_CHANNEL15, the digital channels that are supported.
    Public Const PICO_INVALID_DIGITAL_CHANNEL = &H114

    ' The digital trigger direction Is Not a valid trigger direction And should be equal
    ' in value to one of the <API>_DIGITAL_DIRECTION enumerations.
    Public Const PICO_INVALID_DIGITAL_TRIGGER_DIRECTION = &H115

    ' Signal generator does Not generate pseudo-random binary sequence.
    Public Const PICO_SIG_GEN_PRBS_NOT_SUPPORTED = &H116

    ' When a digital port Is enabled, ETS sample mode Is Not available for use.
    Public Const PICO_ETS_NOT_AVAILABLE_WITH_LOGIC_CHANNELS = &H117

    Public Const PICO_WARNING_REPEAT_VALUE = &H118

    ' 4-channel scopes only: The DC power supply is connected.
    Public Const PICO_POWER_SUPPLY_CONNECTED = &H119

    ' 4-channel scopes only: The DC power supply is not connected.
    Public Const PICO_POWER_SUPPLY_NOT_CONNECTED = &H11A

    ' Incorrect power mode passed for current power source.
    Public Const PICO_POWER_SUPPLY_REQUEST_INVALID = &H11B

    ' The supply voltage from the USB source is too low.
    Public Const PICO_POWER_SUPPLY_UNDERVOLTAGE = &H11C

    ' The oscilloscope is in the process of capturing data.
    Public Const PICO_CAPTURING_DATA = &H11D

    ' A USB 3.0 device is connected to a non-USB 3.0 port.
    Public Const PICO_USB3_0_DEVICE_NON_USB3_0_PORT = &H11E

    ' A function has been called that is not supported by the current device.
    Public Const PICO_NOT_SUPPORTED_BY_THIS_DEVICE = &H11F

    ' The device resolution is invalid (out of range).
    Public Const PICO_INVALID_DEVICE_RESOLUTION = &H120

    ' The number of channels that can be enabled is limited in 15 and 16-bit modes. (Flexible Resolution Oscilloscopes only)
    Public Const PICO_INVALID_NUMBER_CHANNELS_FOR_RESOLUTION = &H121

    ' USB power not sufficient for all requested channels.
    Public Const PICO_CHANNEL_DISABLED_DUE_TO_USB_POWERED = &H122

    ' The signal generator does Not have a configurable DC offset.
    Public Const PICO_SIGGEN_DC_VOLTAGE_NOT_CONFIGURABLE = &H123

    ' An attempt has been made to define pre-trigger delay without first enabling a trigger.
    Public Const PICO_NO_TRIGGER_ENABLED_FOR_TRIGGER_IN_PRE_TRIG = &H124

    ' An attempt has been made to define pre-trigger delay without first arming a trigger.
    Public Const PICO_TRIGGER_WITHIN_PRE_TRIG_NOT_ARMED = &H125

    ' Pre-trigger delay And post-trigger delay cannot be used at the same time.
    Public Const PICO_TRIGGER_WITHIN_PRE_NOT_ALLOWED_WITH_DELAY = &H126

    ' The array index points to a nonexistent trigger.
    Public Const PICO_TRIGGER_INDEX_UNAVAILABLE = &H127

    Public Const PICO_AWG_CLOCK_FREQUENCY = &H128

    ' There are more 4 analog channels with a trigger condition set.
    Public Const PICO_TOO_MANY_CHANNELS_IN_USE = &H129

    ' The condition parameter Is a null pointer.
    Public Const PICO_NULL_CONDITIONS = &H12A

    ' There Is more than one condition pertaining to the same channel.
    Public Const PICO_DUPLICATE_CONDITION_SOURCE = &H12B

    ' The parameter relating to condition information Is out of range.
    Public Const PICO_INVALID_CONDITION_INFO = &H12C

    ' Reading the metadata has failed.
    Public Const PICO_SETTINGS_READ_FAILED = &H12D

    ' Writing the metadata has failed.
    Public Const PICO_SETTINGS_WRITE_FAILED = &H12E

    ' A parameter has a value out of the expected range.
    Public Const PICO_ARGUMENT_OUT_OF_RANGE = &H12F

    ' The driver does Not support the hardware variant connected.
    Public Const PICO_HARDWARE_VERSION_NOT_SUPPORTED = &H130

    ' The driver does Not support the digital hardware variant connected.
    Public Const PICO_DIGITAL_HARDWARE_VERSION_NOT_SUPPORTED = &H131

    ' The driver does Not support the analog hardware variant connected.
    Public Const PICO_ANALOGUE_HARDWARE_VERSION_NOT_SUPPORTED = &H132

    ' Converting a channel's ADC value to resistance has failed.
    Public Const PICO_UNABLE_TO_CONVERT_TO_RESISTANCE = &H133

    ' The channel Is listed more than once in the function call.
    Public Const PICO_DUPLICATED_CHANNEL = &H134

    ' The range cannot have resistance conversion applied.
    Public Const PICO_INVALID_RESISTANCE_CONVERSION = &H135

    ' An invalid value Is in the max buffer.
    Public Const PICO_INVALID_VALUE_IN_MAX_BUFFER = &H136

    ' An invalid value Is in the min buffer.
    Public Const PICO_INVALID_VALUE_IN_MIN_BUFFER = &H137

    ' When calculating the frequency for phase conversion,  
    ' the frequency Is greater than that supported by the current variant.
    Public Const PICO_SIGGEN_FREQUENCY_OUT_OF_RANGE = &H138

    ' The device's EEPROM is corrupt. Contact Pico Technology support: https:'www.picotech.com/tech-support.
    Public Const PICO_EEPROM2_CORRUPT = &H139

    ' The EEPROM has failed.
    Public Const PICO_EEPROM2_FAIL = &H13A

    ' The serial buffer Is too small for the required information.
    Public Const PICO_SERIAL_BUFFER_TOO_SMALL = &H13B

    ' The signal generator trigger And the external clock have both been set.
    ' This Is Not allowed.
    Public Const PICO_SIGGEN_TRIGGER_AND_EXTERNAL_CLOCK_CLASH = &H13C

    ' The AUX trigger was enabled And the external clock has been enabled, 
    ' so the AUX has been automatically disabled.
    Public Const PICO_WARNING_SIGGEN_AUXIO_TRIGGER_DISABLED = &H13D

    ' The AUX I/O was set as a scope trigger And Is now being set as a signal generator
    ' gating trigger. This Is Not allowed.
    Public Const PICO_SIGGEN_GATING_AUXIO_NOT_AVAILABLE = &H13E

    ' The AUX I/O was set by the signal generator as a gating trigger And Is now being set 
    ' as a scope trigger. This Is Not allowed.
    Public Const PICO_SIGGEN_GATING_AUXIO_ENABLED = &H13F

    ' A resource has failed to initialise 
    Public Const PICO_RESOURCE_ERROR = &H140

    ' The temperature type Is out of range
    Public Const PICO_TEMPERATURE_TYPE_INVALID = &H141

    ' A requested temperature type Is Not supported on this device
    Public Const PICO_TEMPERATURE_TYPE_NOT_SUPPORTED = &H142

    ' A read/write to the device has timed out
    Public Const PICO_TIMEOUT = &H143

    ' The device cannot be connected correctly
    Public Const PICO_DEVICE_NOT_FUNCTIONING = &H144

    ' The driver has experienced an unknown error And Is unable to recover from this error
    Public Const PICO_INTERNAL_ERROR = &H145

    ' Used when opening units via IP And more than multiple units have the same ip address
    Public Const PICO_MULTIPLE_DEVICES_FOUND = &H146

    Public Const PICO_WARNING_NUMBER_OF_SEGMENTS_REDUCED = &H147

    ' the calibration pin states argument Is out of range
    Public Const PICO_CAL_PINS_STATES = &H148

    ' the calibration pin frequency argument Is out of range
    Public Const PICO_CAL_PINS_FREQUENCY = &H149

    ' the calibration pin amplitude argument Is out of range
    Public Const PICO_CAL_PINS_AMPLITUDE = &H14A

    ' the calibration pin wavetype argument Is out of range
    Public Const PICO_CAL_PINS_WAVETYPE = &H14B

    ' the calibration pin offset argument Is out of range
    Public Const PICO_CAL_PINS_OFFSET = &H14C

    ' the probe's identity has a problem
    Public Const PICO_PROBE_FAULT = &H14D

    ' the probe has Not been identified
    Public Const PICO_PROBE_IDENTITY_UNKNOWN = &H14E

    ' enabling the probe would cause the device to exceed the allowable current limit
    Public Const PICO_PROBE_POWER_DC_POWER_SUPPLY_REQUIRED = &H14F

    ' the DC power supply Is connected; enabling the probe would cause the device to exceed the
    ' allowable current limit
    Public Const PICO_PROBE_NOT_POWERED_WITH_DC_POWER_SUPPLY = &H150

    ' failed to complete probe configuration
    Public Const PICO_PROBE_CONFIG_FAILURE = &H151

    ' failed to set the callback function, as currently in current callback function
    Public Const PICO_PROBE_INTERACTION_CALLBACK = &H152

    ' the probe has been verified but Not know on this driver
    Public Const PICO_UNKNOWN_INTELLIGENT_PROBE = &H153

    ' the intelligent probe cannot be verified
    Public Const PICO_INTELLIGENT_PROBE_CORRUPT = &H154

    ' the callback Is null, probe collection will only start when 
    ' first callback Is a none null pointer
    Public Const PICO_PROBE_COLLECTION_NOT_STARTED = &H155

    ' the current drawn by the probe(s) has exceeded the allowed limit
    Public Const PICO_PROBE_POWER_CONSUMPTION_EXCEEDED = &H156

    ' the channel range limits have changed due to connecting Or disconnecting a probe
    ' the channel has been enabled
    Public Const PICO_WARNING_PROBE_CHANNEL_OUT_OF_SYNC = &H157

    ' The time stamp per waveform segment has been reset.
    Public Const PICO_DEVICE_TIME_STAMP_RESET = &H1000000

    ' An internal erorr has occurred And a watchdog timer has been called.
    Public Const PICO_WATCHDOGTIMER = &H10000000

    ' The picoipp.dll has Not been found.
    Public Const PICO_IPP_NOT_FOUND = &H10000001

    ' A function in the picoipp.dll does Not exist.
    Public Const PICO_IPP_NO_FUNCTION = &H10000002

    ' The Pico IPP call has failed.
    Public Const PICO_IPP_ERROR = &H10000003

    ' Shadow calibration Is Not available on this device.
    Public Const PICO_SHADOW_CAL_NOT_AVAILABLE = &H10000004

    ' Shadow calibration Is currently disabled.
    Public Const PICO_SHADOW_CAL_DISABLED = &H10000005

    ' Shadow calibration error has occurred.
    Public Const PICO_SHADOW_CAL_ERROR = &H10000006

    ' The shadow calibration Is corrupt.
    Public Const PICO_SHADOW_CAL_CORRUPT = &H10000007

    ' The memory onboard the device has overflowed
    Public Const PICO_DEVICE_MEMORY_OVERFLOW = &H10000008

    Public Const PICO_RESERVED_1 = &H11000000

End Module
