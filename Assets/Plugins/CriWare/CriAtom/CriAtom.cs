<<<<<<< HEAD
﻿/****************************************************************************
 *
 * Copyright (c) 2011 CRI Middleware Co., Ltd.
 *
 ****************************************************************************/

/*---------------------------
 * Force Load Data with Async Method Defines
 *---------------------------*/
#if !(!UNITY_EDITOR && UNITY_IOS && ENABLE_MONO)
	#define CRIWARE_SUPPORT_NATIVE_CALLBACK
#endif

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.IO;
#if !UNITY_EDITOR && (UNITY_WINRT && !ENABLE_IL2CPP)
using System.Reflection;
#endif

public static class CriAtomPlugin
{
	#region Editor/Runtime共通

#if UNITY_EDITOR
	public static bool showDebugLog = false;
	public delegate void PreviewCallback();
	public static PreviewCallback previewCallback = null;
#endif

	public static void Log(string log)
	{
	#if UNITY_EDITOR
		if (CriAtomPlugin.showDebugLog) {
			Debug.Log(log);
		}
	#endif
	}

	/* 初期化カウンタ */
	private static int initializationCount = 0;

	public static bool isInitialized { get { return initializationCount > 0; } }

    private static List<IntPtr> effectInterfaceList = null;
    public static bool GetAudioEffectInterfaceList(out List<IntPtr> effect_interface_list) {
        if (CriAtomPlugin.IsLibraryInitialized() == true) {
            effect_interface_list = null;
            return false;
        }
        if (effectInterfaceList == null) {
            effectInterfaceList = new List<IntPtr>();
        }
        effect_interface_list = effectInterfaceList;
        return true;
    }

    public static void SetConfigParameters(int max_virtual_voices,
		int max_voice_limit_groups, int max_categories,
		int max_sequence_events_per_frame, int max_beatsync_callbacks_per_frame,
		int num_standard_memory_voices, int num_standard_streaming_voices,
		int num_hca_mx_memory_voices, int num_hca_mx_streaming_voices,
		int output_sampling_rate, int num_asr_output_channels,
		bool uses_in_game_preview, float server_frequency,
		int max_parameter_blocks,  int categories_per_playback,
		int num_buses, bool vr_mode)
    {
        criAtomUnity_SetConfigParameters(max_virtual_voices,
			max_voice_limit_groups, max_categories, 
			max_sequence_events_per_frame, max_beatsync_callbacks_per_frame,
			num_standard_memory_voices, num_standard_streaming_voices,
			num_hca_mx_memory_voices, num_hca_mx_streaming_voices,
			output_sampling_rate, num_asr_output_channels,
			uses_in_game_preview, server_frequency,
			max_parameter_blocks, categories_per_playback,
			num_buses, vr_mode,
			IntPtr.Zero);

		CriAtomPlugin.isConfigured = true;
	}

	public static void SetConfigAdditionalParameters_PC(long buffering_time_pc)
	{
		criAtomUnity_SetConfigAdditionalParameters_PC(buffering_time_pc);
	}

	public static void SetConfigAdditionalParameters_IOS(uint buffering_time_ios, bool override_ipod_music_ios)
	{
		criAtomUnity_SetConfigAdditionalParameters_IOS(buffering_time_ios, override_ipod_music_ios);
	}

	public static void SetConfigAdditionalParameters_ANDROID(int num_low_delay_memory_voices, int num_low_delay_streaming_voices,
															 int sound_buffering_time,		  int sound_start_buffering_time,
                                                             IntPtr android_context)
	{
		criAtomUnity_SetConfigAdditionalParameters_ANDROID(num_low_delay_memory_voices, num_low_delay_streaming_voices,
														   sound_buffering_time,		sound_start_buffering_time,
														   android_context);
	}

	public static void SetMaxSamplingRateForStandardVoicePool(int sampling_rate_for_memory, int sampling_rate_for_streaming)
	{
		criAtomUnity_SetMaxSamplingRateForStandardVoicePool(sampling_rate_for_memory, sampling_rate_for_streaming);
	}

	public static int GetRequiredMaxVirtualVoices(CriAtomConfig atomConfig)
	{
		/* バーチャルボイスは、全ボイスプールのボイスの合計値よりも多くなくてはならない */
		int requiredVirtualVoices = 0;

		requiredVirtualVoices += atomConfig.standardVoicePoolConfig.memoryVoices;
		requiredVirtualVoices += atomConfig.standardVoicePoolConfig.streamingVoices;
		requiredVirtualVoices += atomConfig.hcaMxVoicePoolConfig.memoryVoices;
		requiredVirtualVoices += atomConfig.hcaMxVoicePoolConfig.streamingVoices;

		#if UNITY_ANDROID
		requiredVirtualVoices += atomConfig.androidLowLatencyStandardVoicePoolConfig.memoryVoices;
		requiredVirtualVoices += atomConfig.androidLowLatencyStandardVoicePoolConfig.streamingVoices;
		#endif

		return requiredVirtualVoices;
	}

	public static void InitializeLibrary()
	{
		/* 初期化カウンタの更新 */
		CriAtomPlugin.initializationCount++;
		if (CriAtomPlugin.initializationCount != 1) {
			return;
		}

		/* シーン実行前に初期化済みの場合は終了させる */
		if (CriAtomPlugin.IsLibraryInitialized() == true) {
			CriAtomPlugin.FinalizeLibrary();
			CriAtomPlugin.initializationCount = 1;
		}

		/* 初期化パラメータが設定済みかどうかを確認 */
		if (CriAtomPlugin.isConfigured == false) {
			Debug.Log("[CRIWARE] Atom initialization parameters are not configured. "
				+ "Initializes Atom by default parameters.");
		}

		/* 依存ライブラリの初期化 */
		CriFsPlugin.InitializeLibrary();

		/* ライブラリの初期化 */
		CriAtomPlugin.criAtomUnity_Initialize();
	
		/* CriAtomServerのインスタンスを生成 */
		#if UNITY_EDITOR
		/* ゲームプレビュー時のみ生成する */
		if (UnityEngine.Application.isPlaying) {
			CriAtomServer.CreateInstance();
		}
		#else
		CriAtomServer.CreateInstance();
		#endif

		/* CriAtomListener の共有ネイティブリスナーを生成 */
		CriAtomListener.CreateSharedNativeListener();
	}

    public static bool IsLibraryInitialized()
    {
        /* ライブラリが初期化済みかチェック */
        return criAtomUnity_IsInitialized();
    }

	public static void FinalizeLibrary()
	{
		/* 初期化カウンタの更新 */
		CriAtomPlugin.initializationCount--;
		if (CriAtomPlugin.initializationCount < 0) {
			CriAtomPlugin.initializationCount = 0;
			if (CriAtomPlugin.IsLibraryInitialized() == false) {
				return;
			}
		}
		if (CriAtomPlugin.initializationCount != 0) {
			return;
		}

		/* CriAtomListener の共有ネイティブリスナーを破棄 */
		CriAtomListener.DestroySharedNativeListener();

		/* CriAtomServerのインスタンスを破棄 */
		CriAtomServer.DestroyInstance();

		/* 未破棄のDisposableを破棄 */
		CriDisposableObjectManager.CallOnModuleFinalization(CriDisposableObjectManager.ModuleType.Atom);

        /* ユーザエフェクトインタフェースのリストをクリア */
        if (effectInterfaceList != null) {
            effectInterfaceList.Clear();
            effectInterfaceList = null;
        }

		/* ライブラリの終了 */
		CriAtomPlugin.criAtomUnity_Finalize();

		/* 依存ライブラリの終了 */
		CriFsPlugin.FinalizeLibrary();
	}

	public static void Pause(bool pause)
	{
		if (isInitialized) {
			criAtomUnity_Pause(pause);
		}
	}

	#if !UNITY_EDITOR && UNITY_IOS
	public static void CallOnApplicationResume_IOS()
	{
		criAtomUnity_SleepToDelay_IOS(100);
	}
	#endif

	private static bool isConfigured = false;
	private static float timeSinceStartup = Time.realtimeSinceStartup;
	private static CriWare.CpuUsage cpuUsage;
	public static CriWare.CpuUsage GetCpuUsage()
	{
		float currentTimeSinceStartup = Time.realtimeSinceStartup;
		if (currentTimeSinceStartup - timeSinceStartup > 1.0f) {
			CriAtomEx.PerformanceInfo info;
			CriAtomEx.GetPerformanceInfo(out info);

			cpuUsage.last = info.lastServerTime * 100.0f / info.averageServerInterval;
			cpuUsage.average = info.averageServerTime * 100.0f / info.averageServerInterval;
			cpuUsage.peak = info.maxServerTime * 100.0f / info.averageServerInterval;

			CriAtomEx.ResetPerformanceMonitor();
			timeSinceStartup = currentTimeSinceStartup;
		}
		return cpuUsage;
	}

	private static int CRIATOMUNITY_PARAMETER_ID_LOOP_COUNT = 0;
	private static ushort CRIATOMPARAMETER2_ID_INVALID = ushort.MaxValue;

	public static ushort GetLoopCountParameterId()
	{
		ushort ret = criAtomUnity_GetNativeParameterId(CRIATOMUNITY_PARAMETER_ID_LOOP_COUNT);
		if (ret == CRIATOMPARAMETER2_ID_INVALID) {
			throw new Exception("GetNativeParameterId failed.");
		}
		return ret;
	}

	#region DLL Import
	#if !CRIWARE_ENABLE_HEADLESS_MODE
    [DllImport(CriWare.pluginName, CallingConvention = CriWare.pluginCallingConvention)]
    private static extern void criAtomUnity_SetConfigParameters(int max_virtual_voices,
        int max_voice_limit_groups, int max_categories,
		int max_sequence_events_per_frame, int max_beatsync_callbacks_per_frame,
        int num_standard_memory_voices, int num_standard_streaming_voices,
        int num_hca_mx_memory_voices, int num_hca_mx_streaming_voices,
        int output_sampling_rate, int num_asr_output_channels,
        bool uses_in_game_preview, float server_frequency,
        int max_parameter_blocks, int categories_per_playback,
        int num_buses, bool use_ambisonics,
        IntPtr spatializer_core_interface);

	[DllImport(CriWare.pluginName, CallingConvention = CriWare.pluginCallingConvention)]
	private static extern void criAtomUnity_SetConfigAdditionalParameters_PC(long buffering_time_pc);

	[DllImport(CriWare.pluginName, CallingConvention = CriWare.pluginCallingConvention)]
	private static extern void criAtomUnity_SetConfigAdditionalParameters_IOS(uint buffering_time_ios, bool override_ipod_music_ios);

	[DllImport(CriWare.pluginName, CallingConvention = CriWare.pluginCallingConvention)]
	private static extern void criAtomUnity_SetConfigAdditionalParameters_ANDROID(int num_low_delay_memory_voices, int num_low_delay_streaming_voices,
																				  int sound_buffering_time,		   int sound_start_buffering_time,
																				  IntPtr android_context);

	[DllImport(CriWare.pluginName, CallingConvention = CriWare.pluginCallingConvention)]
	private static extern void criAtomUnity_Initialize();

    [DllImport(CriWare.pluginName, CallingConvention = CriWare.pluginCallingConvention)]
    public static extern bool criAtomUnity_IsInitialized();

	[DllImport(CriWare.pluginName, CallingConvention = CriWare.pluginCallingConvention)]
	private static extern void criAtomUnity_Finalize();

	[DllImport(CriWare.pluginName, CallingConvention = CriWare.pluginCallingConvention)]
	private static extern void criAtomUnity_Pause(bool pause);

	[DllImport(CriWare.pluginName, CallingConvention = CriWare.pluginCallingConvention)]
	public static extern uint criAtomUnity_GetAllocatedHeapSize();

	[DllImport(CriWare.pluginName, CallingConvention = CriWare.pluginCallingConvention)]
	public static extern void criAtomUnity_ControlDataCompatibility(int code);

	[DllImport(CriWare.pluginName, CallingConvention = CriWare.pluginCallingConvention)]
	public static extern void criAtomUnitySequencer_SetEventCallback(IntPtr cbfunc, string separator_string);

	[DllImport(CriWare.pluginName, CallingConvention = CriWare.pluginCallingConvention)]
	public static extern void criAtomUnitySequencer_ExecuteQueuedEventCallbacks();

	[DllImport(CriWare.pluginName, CallingConvention = CriWare.pluginCallingConvention)]
	public static extern void criAtomUnity_SetBeatSyncCallback(IntPtr cbfunc);

	[DllImport(CriWare.pluginName, CallingConvention = CriWare.pluginCallingConvention)]
	public static extern void criAtomUnity_ExecuteQueuedBeatSyncCallbacks();

	[DllImport(CriWare.pluginName, CallingConvention = CriWare.pluginCallingConvention)]
	private static extern void criAtomUnity_SetMaxSamplingRateForStandardVoicePool(int sampling_rate_for_memory, int sampling_rate_for_streaming);

	[DllImport(CriWare.pluginName, CallingConvention = CriWare.pluginCallingConvention)]
	public static extern void criAtomUnity_BeginLeCompatibleMode();

	[DllImport(CriWare.pluginName, CallingConvention = CriWare.pluginCallingConvention)]
	public static extern void criAtomUnity_EndLeCompatibleMode();

	[DllImport(CriWare.pluginName, CallingConvention = CriWare.pluginCallingConvention)]
	public static extern ushort criAtomUnity_GetNativeParameterId(int id);

	#if !UNITY_EDITOR && UNITY_IOS
	[DllImport(CriWare.pluginName, CallingConvention = CriWare.pluginCallingConvention)]
	public static extern void criAtomUnity_SleepToDelay_IOS(int milliseconds);
	#endif
	#else
	private static void criAtomUnity_SetConfigParameters(int max_virtual_voices,
		int max_voice_limit_groups, int max_categories,
		int max_sequence_events_per_frame, int max_beatsync_callbacks_per_frame,
		int num_standard_memory_voices, int num_standard_streaming_voices,
		int num_hca_mx_memory_voices, int num_hca_mx_streaming_voices,
		int output_sampling_rate, int num_asr_output_channels,
		bool uses_in_game_preview, float server_frequency,
		int max_parameter_blocks, int categories_per_playback,
		int num_buses, bool use_ambisonics,
		IntPtr spatializer_core_interface) { }
	private static void criAtomUnity_SetConfigAdditionalParameters_PC(long buffering_time_pc) { }
	private static void criAtomUnity_SetConfigAdditionalParameters_IOS(uint buffering_time_ios, bool override_ipod_music_ios) { }
	private static void criAtomUnity_SetConfigAdditionalParameters_ANDROID(int num_low_delay_memory_voices, int num_low_delay_streaming_voices,
																			int sound_buffering_time,		   int sound_start_buffering_time,
																			IntPtr android_context) { }
	private static bool _dummyInitialized = false;
	private static void criAtomUnity_Initialize() { _dummyInitialized = true; }
	public static bool criAtomUnity_IsInitialized() { return _dummyInitialized; }
	private static void criAtomUnity_Finalize() { _dummyInitialized = false; }
	private static void criAtomUnity_Pause(bool pause) { }
	public static uint criAtomUnity_GetAllocatedHeapSize() { return 0; }
	public static void criAtomUnity_ControlDataCompatibility(int code) { }
	public static void criAtomUnitySequencer_SetEventCallback(IntPtr cbfunc, string separator_string) { }
	public static void criAtomUnitySequencer_ExecuteQueuedEventCallbacks() { }
	public static void criAtomUnity_SetBeatSyncCallback(IntPtr cbfunc) { }
	public static void criAtomUnity_ExecuteQueuedBeatSyncCallbacks() { }
	private static void criAtomUnity_SetMaxSamplingRateForStandardVoicePool(int sampling_rate_for_memory, int sampling_rate_for_streaming) { }
	public static void criAtomUnity_BeginLeCompatibleMode() { }
	public static void criAtomUnity_EndLeCompatibleMode() { }
	public static ushort criAtomUnity_GetNativeParameterId(int id) { return 0; }
#if !UNITY_EDITOR && UNITY_IOS
	public static void criAtomUnity_SleepToDelay_IOS(int milliseconds) { }
#endif
	#endif
	#endregion

	#endregion
}

[Serializable]
public class CriAtomCueSheet
{
	public string name = "";
	public string acbFile = "";
	public string awbFile = "";
	public CriAtomExAcb acb;
	public CriAtomExAcbLoader.Status loaderStatus = CriAtomExAcbLoader.Status.Stop;
	public bool IsLoading { get { return loaderStatus == CriAtomExAcbLoader.Status.Loading; } }
    public bool IsError { get { return (loaderStatus == CriAtomExAcbLoader.Status.Error) || (!IsLoading && acb == null); } }
}

/// \addtogroup CRIATOM_UNITY_COMPONENT
/// @{
/**
 * <summary>サウンド再生全体を制御するためのコンポーネントです。</summary>
 * \par 説明:
 * 各シーンにひとつ用意しておく必要があります。<br/>
 * UnityEditor上でCRI Atomウィンドウを使用して CriAtomSource を作成した場合、
 * 「CRIWARE」という名前を持つオブジェクトとして自動的に作成されます。通常はユーザーが作成する必要はありません。
 */
[AddComponentMenu("CRIWARE/CRI Atom")]
public class CriAtom : MonoBehaviour
{

	/* @cond DOXYGEN_IGNORE */
	public string acfFile = "";
	private bool acfIsLoading = false;
#if CRIWARE_FORCE_LOAD_ASYNC
	private byte[] acfData = null;
#endif
	public CriAtomCueSheet[] cueSheets = {};
	public string dspBusSetting = "";
	public bool dontDestroyOnLoad = false;
#if CRIWARE_SUPPORT_NATIVE_CALLBACK
	private static CriAtomExSequencer.EventCbFunc eventUserCbFunc = null;
	private static CriAtomExBeatSync.CbFunc beatsyncUserCbFunc = null;
#endif
	private static CriAtom instance {
		get; set;
	}

	/* @endcond */

	#region Functions

	/**
	 * <summary>DSPバス設定のアタッチ</summary>
	 * <param name="settingName">DSPバス設定の名前</param>
	 * \par 説明:
	 * DSPバス設定からDSPバスを構築してサウンドレンダラにアタッチします。<br/>
	 * 現在設定中のDSPバス設定を切り替える場合は、一度デタッチしてから再アタッチしてください。
	 * <br/>
	 * \attention
	 * 本関数は完了復帰型の関数です。<br/>
	 * 本関数を実行すると、しばらくの間Atomライブラリのサーバ処理がブロックされます。<br/>
	 * 音声再生中に本関数を実行すると、音途切れ等の不具合が発生する可能性があるため、
	 * 本関数の呼び出しはシーンの切り替わり等、負荷変動を許容できるタイミングで行ってください。<br/>
	 * \sa CriAtom::DetachDspBusSetting
	 */
	public static void AttachDspBusSetting(string settingName)
	{
		CriAtom.instance.dspBusSetting = settingName;
		if (!String.IsNullOrEmpty(settingName)) {
			CriAtomEx.AttachDspBusSetting(settingName);
		} else {
			CriAtomEx.DetachDspBusSetting();
		}
	}

	/**
	 * <summary>DSPバス設定のデタッチ</summary>
	 * \par 説明:
	 * DSPバス設定をデタッチします。<br/>
	 * <br/>
	 * \attention
	 * 本関数は完了復帰型の関数です。<br/>
	 * 本関数を実行すると、しばらくの間Atomライブラリのサーバ処理がブロックされます。<br/>
	 * 音声再生中に本関数を実行すると、音途切れ等の不具合が発生する可能性があるため、
	 * 本関数の呼び出しはシーンの切り替わり等、負荷変動を許容できるタイミングで行ってください。<br/>
	 * \sa CriAtom::DetachDspBusSetting
	 */
	public static void DetachDspBusSetting()
	{
		CriAtom.instance.dspBusSetting = "";
		CriAtomEx.DetachDspBusSetting();
	}

	/**
	 * <summary>キューシートの取得</summary>
	 * <param name="name">キューシート名</param>
	 * <returns>キューシートオブジェクト</returns>
	 * \par 説明:
	 * 引数に指定したキューシート名を元に登録済みのキューシートオブジェクトを取得します。<br/>
	 */
	public static CriAtomCueSheet GetCueSheet(string name)
	{
		return CriAtom.instance.GetCueSheetInternal(name);
	}

	/**
	 * <summary>キューシートの追加</summary>
	 * <param name="name">キューシート名</param>
	 * <param name="acbFile">ACBファイルパス</param>
	 * <param name="awbFile">AWBファイルパス</param>
	 * <param name="binder">バインダオブジェクト(オプション)</param>
	 * <returns>キューシートオブジェクト</returns>
	 * \par 説明:
	 * 引数に指定したファイルパス情報を元にキューシートの追加を行います。<br/>
	 * 同時に複数のキューシートを登録することが可能です。<br/>
	 * <br/>
	 * 各ファイルパスに対して相対パスを指定した場合は StreamingAssets フォルダからの相対でファイルをロードします。<br/>
	 * 絶対パス、あるいはURLを指定した場合には指定したパスでファイルをロードします。<br/>
	 * <br/>
	 * CPKファイルにパッキングされたACBファイルとAWBファイルからキューシートを追加する場合は、
	 * binder引数にCPKをバインドしたバインダを指定してください。<br/>
	 * なお、バインダ機能はADX2LEではご利用になれません。<br/>
	 */
	public static CriAtomCueSheet AddCueSheet(string name, string acbFile, string awbFile, CriFsBinder binder = null)
	{
	#if CRIWARE_FORCE_LOAD_ASYNC
		return CriAtom.AddCueSheetAsync(name, acbFile, awbFile, binder);
	#else
		CriAtomCueSheet cueSheet = CriAtom.instance.AddCueSheetInternal(name, acbFile, awbFile, binder);
		if (Application.isPlaying) {
			cueSheet.acb = CriAtom.instance.LoadAcbFile(binder, acbFile, awbFile);
		}
		return cueSheet;
	#endif
	}

	/**
	 * <summary>非同期でのキューシートの追加</summary>
	 * <param name="name">キューシート名</param>
	 * <param name="acbFile">ACBファイルパス</param>
	 * <param name="awbFile">AWBファイルパス</param>
	 * <param name="binder">バインダオブジェクト(オプション)</param>
	 * <param name="loadAwbOnMemory">AWBファイルをメモリ上にロードするか(オプション)</param>
	 * <returns>キューシートオブジェクト</returns>
	 * \par 説明:
	 * 引数に指定したファイルパス情報を元に、非同期でキューシートの追加を行います。<br/>
	 * 同時に複数のキューシートを登録することが可能です。<br/>
	 * <br/>
	 * 各ファイルパスに対して相対パスを指定した場合は StreamingAssets フォルダからの相対でファイルをロードします。<br/>
	 * 絶対パス、あるいはURLを指定した場合には指定したパスでファイルをロードします。<br/>
	 * <br/>
	 * CPKファイルにパッキングされたACBファイルとAWBファイルからキューシートを追加する場合は、
	 * binder引数にCPKをバインドしたバインダを指定してください。<br/>
	 * なお、バインダ機能はADX2LEではご利用になれません。<br/>
	 * <br/>
	 * 戻り値のキューシートオブジェクトの CriAtomCueSheet::isLoading メンバが true を返す間はロード中です。<br/>
	 * 必ず false を返すのを確認してからキューの再生等を行うようにしてください。<br/>
	 * <br/>
	 * loadAwbOnMemory が false の場合、AWBファイルのヘッダ部分のみをメモリ上にロードしストリーム再生を行います。<br/>
	 * loadAwbOnMemory を true に設定すると、AWBファイル全体をメモリ上にロードするため実質メモリ再生になります。<br/>
	 */
	public static CriAtomCueSheet AddCueSheetAsync(string name, string acbFile, string awbFile, CriFsBinder binder = null, bool loadAwbOnMemory = false)
	{
	#if UNITY_EDITOR && UNITY_WEBGL
		loadAwbOnMemory = true;
	#endif
		CriAtomCueSheet cueSheet = CriAtom.instance.AddCueSheetInternal(name, acbFile, awbFile, binder);
		if (Application.isPlaying) {
			CriAtom.instance.LoadAcbFileAsync(cueSheet, binder, acbFile, awbFile, loadAwbOnMemory);
		}
		return cueSheet;
	}

	/**
	 * <summary>キューシートの追加（メモリからの読み込み）</summary>
	 * <param name="name">キューシート名</param>
	 * <param name="acbData">ACBデータ</param>
	 * <param name="awbFile">AWBファイルパス</param>
	 * <param name="awbBinder">AWB用バインダオブジェクト(オプション)</param>
	 * <returns>キューシートオブジェクト</returns>
	 * \par 説明:
	 * 引数に指定したデータとファイルパス情報からキューシートの追加を行います。<br/>
	 * 同時に複数のキューシートを登録することが可能です。<br/>
	 * <br/>
	 * ファイルパスに対して相対パスを指定した場合は StreamingAssets フォルダからの相対でファイルをロードします。<br/>
	 * 絶対パス、あるいはURLを指定した場合には指定したパスでファイルをロードします。<br/>
	 * <br/>
	 * CPKファイルにパッキングされたAWBファイルからキューシートを追加する場合は、
	 * awbBinder引数にCPKをバインドしたバインダを指定してください。<br/>
	 * なお、バインダ機能はADX2LEではご利用になれません。<br/>
	 */
	public static CriAtomCueSheet AddCueSheet(string name, byte[] acbData, string awbFile, CriFsBinder awbBinder = null)
	{
	#if CRIWARE_FORCE_LOAD_ASYNC
		return CriAtom.AddCueSheetAsync(name, acbData, awbFile, awbBinder);
	#else
		CriAtomCueSheet cueSheet = CriAtom.instance.AddCueSheetInternal(name, "", awbFile, awbBinder);
		if (Application.isPlaying) {
			cueSheet.acb = CriAtom.instance.LoadAcbData(acbData, awbBinder, awbFile);
		}
		return cueSheet;
	#endif
	}

	/**
	 * <summary>非同期でのキューシートの追加（メモリからの読み込み）</summary>
	 * <param name="name">キューシート名</param>
	 * <param name="acbData">ACBデータ</param>
	 * <param name="awbFile">AWBファイルパス</param>
	 * <param name="awbBinder">AWB用バインダオブジェクト(オプション)</param>
 	 * <param name="loadAwbOnMemory">AWBファイルをメモリ上にロードするか(オプション)</param>
	 * <returns>キューシートオブジェクト</returns>
	 * \par 説明:
	 * 引数に指定したデータとファイルパス情報からキューシートの追加を行います。<br/>
	 * 同時に複数のキューシートを登録することが可能です。<br/>
	 * <br/>
	 * ファイルパスに対して相対パスを指定した場合は StreamingAssets フォルダからの相対でファイルをロードします。<br/>
	 * 絶対パス、あるいはURLを指定した場合には指定したパスでファイルをロードします。<br/>
	 * <br/>
	 * CPKファイルにパッキングされたAWBファイルからキューシートを追加する場合は、
	 * awbBinder引数にCPKをバインドしたバインダを指定してください。<br/>
	 * なお、バインダ機能はADX2LEではご利用になれません。<br/>
	 * <br/>
	 * 戻り値のキューシートオブジェクトの CriAtomCueSheet::isLoading メンバが true を返す間はロード中です。<br/>
	 * 必ず false を返すのを確認してからキューの再生等を行うようにしてください。<br/>
	 * <br/>
	 * loadAwbOnMemory が false の場合、AWBファイルのヘッダ部分のみをメモリ上にロードしストリーム再生を行います。<br/>
	 * loadAwbOnMemory を true に設定すると、AWBファイル全体をメモリ上にロードするため実質メモリ再生になります。<br/>
	 */
	public static CriAtomCueSheet AddCueSheetAsync(string name, byte[] acbData, string awbFile, CriFsBinder awbBinder = null, bool loadAwbOnMemory = false)
	{
	#if UNITY_EDITOR && UNITY_WEBGL
		loadAwbOnMemory = true;
	#endif
		CriAtomCueSheet cueSheet = CriAtom.instance.AddCueSheetInternal(name, "", awbFile, awbBinder);
		if (Application.isPlaying) {
			CriAtom.instance.LoadAcbDataAsync(cueSheet, acbData, awbBinder, awbFile, loadAwbOnMemory);
		}
		return cueSheet;
	}

	/**
	 * <summary>キューシートの削除</summary>
	 * <param name="name">キューシート名</param>
	 * \par 説明:
	 * 追加済みのキューシートを削除します。<br/>
	 */
	public static void RemoveCueSheet(string name)
	{
        if (CriAtom.instance == null) {
            return;
        }
		CriAtom.instance.RemoveCueSheetInternal(name);
	}

	/**
	 * <summary>キューシートのロード完了チェック</summary>
	 * \par 説明:
	 * 全てのキューシートのロード完了をチェックします。<br/>
	 */
	public static bool CueSheetsAreLoading {
		get {
			if (CriAtom.instance == null) {
				return false;
			}
			foreach (var cueSheet in CriAtom.instance.cueSheets) {
				if (cueSheet.IsLoading) {
					return true;
				}
			}
			return false;
		}
	}

	/**
	 * <summary>ACBオブジェクトの取得</summary>
	 * <param name="cueSheetName">キューシート名</param>
	 * <returns>ACBオブジェクト</returns>
	 * \par 説明:
	 * 指定したキューシートに対応するACBオブジェクトを取得します。<br/>
	 */
	public static CriAtomExAcb GetAcb(string cueSheetName)
	{
		foreach (var cueSheet in CriAtom.instance.cueSheets) {
			if (cueSheetName == cueSheet.name) {
				return cueSheet.acb;
			}
		}
		Debug.LogWarning(cueSheetName + " is not loaded.");
		return null;
	}

	/**
	 * <summary>カテゴリ名指定でカテゴリボリュームを設定します。</summary>
	 * <param name="name">カテゴリ名</param>
	 * <param name="volume">ボリューム</param>
	 */
	public static void SetCategoryVolume(string name, float volume)
	{
		CriAtomExCategory.SetVolume(name, volume);
	}

	/**
	 * <summary>カテゴリID指定でカテゴリボリュームを設定します。</summary>
	 * <param name="id">カテゴリID</param>
	 * <param name="volume">ボリューム</param>
	 */
	public static void SetCategoryVolume(int id, float volume)
	{
		CriAtomExCategory.SetVolume(id, volume);
	}

	/**
	 * <summary>カテゴリ名指定でカテゴリボリュームを取得します。</summary>
	 * <param name="name">カテゴリ名</param>
	 * <returns>ボリューム</returns>
	 */
	public static float GetCategoryVolume(string name)
	{
		return CriAtomExCategory.GetVolume(name);
	}
	/**
	 * <summary>カテゴリID指定でカテゴリボリュームを取得します。</summary>
	 * <param name="id">カテゴリID</param>
	 * <returns>ボリューム</returns>
	 */
	public static float GetCategoryVolume(int id)
	{
		return CriAtomExCategory.GetVolume(id);
	}

	/**
	 * <summary>バス情報取得を有効にします。</summary>
	 * <param name="busName">DSPバス名</param>
	 * <param name="sw">true: 取得を有効にする。 false: 取得を無効にする</param>
	 */
	public static void SetBusAnalyzer(string busName, bool sw)
	{
		if (sw) {
			CriAtomExAsr.AttachBusAnalyzer(busName, 50, 1000);
		} else {
			CriAtomExAsr.DetachBusAnalyzer(busName);
		}
	}

	/**
	 * <summary>全てのバス情報取得を有効にします。</summary>
	 * <param name="sw">true: 取得を有効にする。 false: 取得を無効にする</param>
	 */
	public static void SetBusAnalyzer(bool sw)
	{
		if (sw) {
			CriAtomExAsr.AttachBusAnalyzer(50, 1000);
		} else {
			CriAtomExAsr.DetachBusAnalyzer();
		}
	}

	/**
	 * <summary>バス情報を取得します。</summary>
	 * <param name="busName">DSPバス名</param>
	 * <returns>DSPバス情報</returns>
	 */
	public static CriAtomExAsr.BusAnalyzerInfo GetBusAnalyzerInfo(string busName)
	{
		CriAtomExAsr.BusAnalyzerInfo info;
		CriAtomExAsr.GetBusAnalyzerInfo(busName, out info);
		return info;
	}

	[System.Obsolete("Use CriAtom.GetBusAnalyzerInfo(string busName)")]
	public static CriAtomExAsr.BusAnalyzerInfo GetBusAnalyzerInfo(int busId)
	{
		CriAtomExAsr.BusAnalyzerInfo info;
		CriAtomExAsr.GetBusAnalyzerInfo(busId, out info);
		return info;
	}

	#endregion // Functions

	/* @cond DOXYGEN_IGNORE */
	#region Functions for internal use

	public void Setup()
	{
		if (CriAtom.instance != null && CriAtom.instance != this) {
			var obj = CriAtom.instance.gameObject;
			CriAtom.instance.Shutdown();
			CriAtomEx.UnregisterAcf();
			GameObject.Destroy(obj);
		}

		CriAtom.instance = this;

		CriAtomPlugin.InitializeLibrary();

		if (!String.IsNullOrEmpty(this.acfFile)) {
			string acfPath = Path.Combine(CriWare.streamingAssetsPath, this.acfFile);
			CriAtomEx.RegisterAcf(null, acfPath);
		}

		if (!String.IsNullOrEmpty(dspBusSetting)) {
			AttachDspBusSetting(dspBusSetting);
		}

		foreach (var cueSheet in this.cueSheets) {
			cueSheet.acb = this.LoadAcbFile(null, cueSheet.acbFile, cueSheet.awbFile);
		}

		if (this.dontDestroyOnLoad) {
			GameObject.DontDestroyOnLoad(this.gameObject);
		}
	}

	public void Shutdown()
	{
		foreach (var cueSheet in this.cueSheets) {
			if (cueSheet.acb != null) {
				cueSheet.acb.Dispose();
				cueSheet.acb = null;
			}
		}
		CriAtomPlugin.FinalizeLibrary();
		if (CriAtom.instance == this) {
			CriAtom.instance = null;
		}
	}

	private void Awake()
	{
		if (CriAtom.instance != null && CriAtom.instance != this) {
			if (CriAtom.instance.acfFile != this.acfFile) {
				var obj = CriAtom.instance.gameObject;
				CriAtom.instance.Shutdown();
				CriAtomEx.UnregisterAcf();
				GameObject.Destroy(obj);
				return;
			}
			if (CriAtom.instance.dspBusSetting != this.dspBusSetting) {
				CriAtom.AttachDspBusSetting(this.dspBusSetting);
			}
			CriAtom.instance.MargeCueSheet(this.cueSheets, this.dontRemoveExistsCueSheet);
			GameObject.Destroy(this.gameObject);
		}
	}

	private void OnEnable()
	{
	#if UNITY_EDITOR
		if (CriAtomPlugin.previewCallback != null) {
			CriAtomPlugin.previewCallback();
		}
	#endif
		if (CriAtom.instance != null) {
			return;
		}

	#if CRIWARE_FORCE_LOAD_ASYNC
		this.SetupAsync();
	#else
		this.Setup();
	#endif
	}

	private void OnDestroy()
	{
		if (this != CriAtom.instance) {
			return;
		}
		this.Shutdown();
	}

	private void Update()
	{
		CriAtomPlugin.criAtomUnitySequencer_ExecuteQueuedEventCallbacks();
		CriAtomPlugin.criAtomUnity_ExecuteQueuedBeatSyncCallbacks();
	}

	public CriAtomCueSheet GetCueSheetInternal(string name)
	{
		for (int i = 0; i < this.cueSheets.Length; i++) {
			CriAtomCueSheet cueSheet = this.cueSheets[i];
			if (cueSheet.name == name) {
				return cueSheet;
			}
		}
		return null;
	}

	public CriAtomCueSheet AddCueSheetInternal(string name, string acbFile, string awbFile, CriFsBinder binder)
	{
		var cueSheets = new CriAtomCueSheet[this.cueSheets.Length + 1];
		this.cueSheets.CopyTo(cueSheets, 0);
		this.cueSheets = cueSheets;

		var cueSheet = new CriAtomCueSheet();
		this.cueSheets[this.cueSheets.Length - 1] = cueSheet;
		if (String.IsNullOrEmpty(name)) {
			cueSheet.name = Path.GetFileNameWithoutExtension(acbFile);
		} else {
			cueSheet.name = name;
		}
		cueSheet.acbFile = acbFile;
		cueSheet.awbFile = awbFile;
		return cueSheet;
	}

	public void RemoveCueSheetInternal(string name)
	{
		int index = -1;
		for (int i = 0; i < this.cueSheets.Length; i++) {
			if (name == this.cueSheets[i].name) {
				index = i;
			}
		}
		if (index < 0) {
			return;
		}

		var cueSheet = this.cueSheets[index];
		if (cueSheet.acb != null) {
			cueSheet.acb.Dispose();
			cueSheet.acb = null;
		}

		var cueSheets = new CriAtomCueSheet[this.cueSheets.Length - 1];
		Array.Copy(this.cueSheets, 0, cueSheets, 0, index);
		Array.Copy(this.cueSheets, index + 1, cueSheets, index, this.cueSheets.Length - index - 1);
		this.cueSheets = cueSheets;
	}

	public bool dontRemoveExistsCueSheet = false;

	/*
	 * newDontRemoveExistsCueSheet == false の場合、古いキューシートを登録解除して、新しいキューシートの登録を行う。
	 * ただし同じキューシートの再登録は回避する
	 */
	private void MargeCueSheet(CriAtomCueSheet[] newCueSheets, bool newDontRemoveExistsCueSheet)
	{
		if (!newDontRemoveExistsCueSheet) {
			for (int i = 0; i < this.cueSheets.Length; ) {
				int index = Array.FindIndex(newCueSheets, sheet => sheet.name == this.cueSheets[i].name);
				if (index < 0) {
					CriAtom.RemoveCueSheet(this.cueSheets[i].name);
				} else {
					i++;
				}
			}
		}

		foreach (var sheet1 in newCueSheets) {
			var sheet2 = this.GetCueSheetInternal(sheet1.name);
			if (sheet2 == null) {
				CriAtom.AddCueSheet(null, sheet1.acbFile, sheet1.awbFile, null);
			}
		}
	}

	private CriAtomExAcb LoadAcbFile(CriFsBinder binder, string acbFile, string awbFile)
	{
		if (String.IsNullOrEmpty(acbFile)) {
			return null;
		}

		string acbPath = acbFile;
		if ((binder == null) && CriWare.IsStreamingAssetsPath(acbPath)) {
			acbPath = Path.Combine(CriWare.streamingAssetsPath, acbPath);
		}

		string awbPath = awbFile;
		if (!String.IsNullOrEmpty(awbPath)) {
			if ((binder == null) && CriWare.IsStreamingAssetsPath(awbPath)) {
				awbPath = Path.Combine(CriWare.streamingAssetsPath, awbPath);
			}
		}

		return CriAtomExAcb.LoadAcbFile(binder, acbPath, awbPath);
	}

	private CriAtomExAcb LoadAcbData(byte[] acbData, CriFsBinder binder, string awbFile)
	{
		if (acbData == null) {
			return null;
		}

		string awbPath = awbFile;
		if (!String.IsNullOrEmpty(awbPath)) {
			if ((binder == null) && CriWare.IsStreamingAssetsPath(awbPath)) {
				awbPath = Path.Combine(CriWare.streamingAssetsPath, awbPath);
			}
		}

		return CriAtomExAcb.LoadAcbData(acbData, binder, awbPath);
	}

#if CRIWARE_FORCE_LOAD_ASYNC
	private void SetupAsync()
	{
		CriAtom.instance = this;

		CriAtomPlugin.InitializeLibrary();

		if (this.dontDestroyOnLoad) {
			GameObject.DontDestroyOnLoad(this.gameObject);
		}

		if (!String.IsNullOrEmpty(this.acfFile)) {
			this.acfIsLoading = true;
			StartCoroutine(RegisterAcfAsync(this.acfFile, this.dspBusSetting));
		}

		foreach (var cueSheet in this.cueSheets) {
			#if UNITY_EDITOR
			bool loadAwbOnMemory = true;
			#else
			bool loadAwbOnMemory = false;
			#endif
			StartCoroutine(LoadAcbFileCoroutine(cueSheet, null, cueSheet.acbFile, cueSheet.awbFile, loadAwbOnMemory));
		}
	}

	private IEnumerator RegisterAcfAsync(string acfFile, string dspBusSetting)
	{
	#if UNITY_EDITOR
		string acfPath = "file://" + CriWare.streamingAssetsPath + "/" + acfFile;
	#else
		string acfPath = CriWare.streamingAssetsPath + "/" + acfFile;
	#endif
		using (var req = new WWW(acfPath)) {
			yield return req;
			this.acfData = req.bytes;
		}
		CriAtomEx.RegisterAcf(this.acfData);

		if (!String.IsNullOrEmpty(dspBusSetting)) {
			AttachDspBusSetting(dspBusSetting);
		}
		this.acfIsLoading = false;
	}
#endif


	private void LoadAcbFileAsync(CriAtomCueSheet cueSheet, CriFsBinder binder, string acbFile, string awbFile, bool loadAwbOnMemory)
	{
		if (String.IsNullOrEmpty(acbFile)) {
			return;
		}

		StartCoroutine(LoadAcbFileCoroutine(cueSheet, binder, acbFile, awbFile, loadAwbOnMemory));
	}

	private IEnumerator LoadAcbFileCoroutine(CriAtomCueSheet cueSheet, CriFsBinder binder, string acbPath, string awbPath, bool loadAwbOnMemory)
	{
		cueSheet.loaderStatus = CriAtomExAcbLoader.Status.Loading;

		if ((binder == null) && CriWare.IsStreamingAssetsPath(acbPath)) {
			acbPath = Path.Combine(CriWare.streamingAssetsPath, acbPath);
		}

		if (!String.IsNullOrEmpty(awbPath)) {
			if ((binder == null) && CriWare.IsStreamingAssetsPath(awbPath)) {
				awbPath = Path.Combine(CriWare.streamingAssetsPath, awbPath);
			}
		}

		while (this.acfIsLoading) {
			yield return new WaitForEndOfFrame();
		}

		using (var asyncLoader = CriAtomExAcbLoader.LoadAcbFileAsync(binder, acbPath, awbPath, loadAwbOnMemory)) {
			if (asyncLoader == null) {
				cueSheet.loaderStatus = CriAtomExAcbLoader.Status.Error;
				yield break;
			}

			while (true) {
				var status = asyncLoader.GetStatus();
				cueSheet.loaderStatus = status;
				if (status == CriAtomExAcbLoader.Status.Complete) {
					cueSheet.acb = asyncLoader.MoveAcb();
					break;
				} else if (status == CriAtomExAcbLoader.Status.Error) {
					break;
				}
				yield return new WaitForEndOfFrame();
			}
		}
	}

	private void LoadAcbDataAsync(CriAtomCueSheet cueSheet, byte[] acbData, CriFsBinder awbBinder, string awbFile, bool loadAwbOnMemory)
	{
		StartCoroutine(LoadAcbDataCoroutine(cueSheet, acbData, awbBinder, awbFile, loadAwbOnMemory));
	}

	private IEnumerator LoadAcbDataCoroutine(CriAtomCueSheet cueSheet, byte[] acbData, CriFsBinder awbBinder, string awbPath, bool loadAwbOnMemory)
	{
		cueSheet.loaderStatus = CriAtomExAcbLoader.Status.Loading;

		if (!String.IsNullOrEmpty(awbPath)) {
			if ((awbBinder == null) && CriWare.IsStreamingAssetsPath(awbPath)) {
				awbPath = Path.Combine(CriWare.streamingAssetsPath, awbPath);
			}
		}

		while (this.acfIsLoading) {
			yield return new WaitForEndOfFrame();
		}

		using (var asyncLoader = CriAtomExAcbLoader.LoadAcbDataAsync(acbData, awbBinder, awbPath, loadAwbOnMemory)) {
			if (asyncLoader == null) {
				cueSheet.loaderStatus = CriAtomExAcbLoader.Status.Error;
				yield break;
			}

			while (true) {
				var status = asyncLoader.GetStatus();
				cueSheet.loaderStatus = status;
				if (status == CriAtomExAcbLoader.Status.Complete) {
					cueSheet.acb = asyncLoader.MoveAcb();
					break;
				} else if (status == CriAtomExAcbLoader.Status.Error) {
					break;
				}
				yield return new WaitForEndOfFrame();
			}
		}
	}

#if CRIWARE_SUPPORT_NATIVE_CALLBACK
	[AOT.MonoPInvokeCallback(typeof(CriAtomExSequencer.EventCbFunc))]
	public static void SequenceEventCallbackFromNative(string eventString)
	{
		/* 本関数はアプリケーションメインスレッドの CriAtom.Update から呼び出される */
		if (eventUserCbFunc != null) {
			eventUserCbFunc(eventString);
		}
	}

	[AOT.MonoPInvokeCallback(typeof(CriAtomExBeatSync.CbFunc))]
	public static void BeatSyncCallbackFromNative(ref CriAtomExBeatSync.Info info)
	{
		/* 本関数はアプリケーションメインスレッドの CriAtom.Update から呼び出される */
		if (beatsyncUserCbFunc != null) {
			beatsyncUserCbFunc(ref info);
		}
	}
#endif

	/* プラグイン内部用API */
	public static void SetEventCallback(CriAtomExSequencer.EventCbFunc func, string separator)
	{
#if CRIWARE_SUPPORT_NATIVE_CALLBACK
		/* ネイティブプラグインに関数ポインタを登録 */
		IntPtr ptr = IntPtr.Zero;
		eventUserCbFunc = func;
		if (func != null) {
			CriAtomExSequencer.EventCbFunc delegateObject;
			delegateObject = new CriAtomExSequencer.EventCbFunc(CriAtom.SequenceEventCallbackFromNative);
			ptr = Marshal.GetFunctionPointerForDelegate(delegateObject);
		}
		CriAtomPlugin.criAtomUnitySequencer_SetEventCallback(ptr, separator);
#else
		Debug.LogError("[CRIWARE] Event callback is not supported for this scripting backend.");
#endif
	}

	public static void SetBeatSyncCallback(CriAtomExBeatSync.CbFunc func)
	{
#if CRIWARE_SUPPORT_NATIVE_CALLBACK
		/* ネイティブプラグインに関数ポインタを登録 */
		IntPtr ptr = IntPtr.Zero;
		beatsyncUserCbFunc = func;
		if (func != null) {
			CriAtomExBeatSync.CbFunc delegateObject;
			delegateObject = new CriAtomExBeatSync.CbFunc (CriAtom.BeatSyncCallbackFromNative);
			ptr = Marshal.GetFunctionPointerForDelegate(delegateObject);
		}
		CriAtomPlugin.criAtomUnity_SetBeatSyncCallback(ptr);
#else
        Debug.LogError("[CRIWARE] Beat sync callback is not supported for this scripting backend.");
#endif
	}

	#endregion
	/* @endcond */

}

/// @}
/* end of file */
=======
﻿/****************************************************************************
 *
 * Copyright (c) 2011 CRI Middleware Co., Ltd.
 *
 ****************************************************************************/

/*---------------------------
 * Force Load Data with Async Method Defines
 *---------------------------*/
#if !(!UNITY_EDITOR && UNITY_IOS && ENABLE_MONO)
	#define CRIWARE_SUPPORT_NATIVE_CALLBACK
#endif

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.IO;
#if !UNITY_EDITOR && (UNITY_WINRT && !ENABLE_IL2CPP)
using System.Reflection;
#endif

public static class CriAtomPlugin
{
	#region Editor/Runtime共通

#if UNITY_EDITOR
	public static bool showDebugLog = false;
	public delegate void PreviewCallback();
	public static PreviewCallback previewCallback = null;
#endif

	public static void Log(string log)
	{
	#if UNITY_EDITOR
		if (CriAtomPlugin.showDebugLog) {
			Debug.Log(log);
		}
	#endif
	}

	/* 初期化カウンタ */
	private static int initializationCount = 0;

	public static bool isInitialized { get { return initializationCount > 0; } }

    private static List<IntPtr> effectInterfaceList = null;
    public static bool GetAudioEffectInterfaceList(out List<IntPtr> effect_interface_list) {
        if (CriAtomPlugin.IsLibraryInitialized() == true) {
            effect_interface_list = null;
            return false;
        }
        if (effectInterfaceList == null) {
            effectInterfaceList = new List<IntPtr>();
        }
        effect_interface_list = effectInterfaceList;
        return true;
    }

    public static void SetConfigParameters(int max_virtual_voices,
		int max_voice_limit_groups, int max_categories,
		int max_sequence_events_per_frame, int max_beatsync_callbacks_per_frame,
		int num_standard_memory_voices, int num_standard_streaming_voices,
		int num_hca_mx_memory_voices, int num_hca_mx_streaming_voices,
		int output_sampling_rate, int num_asr_output_channels,
		bool uses_in_game_preview, float server_frequency,
		int max_parameter_blocks,  int categories_per_playback,
		int num_buses, bool vr_mode)
    {
        criAtomUnity_SetConfigParameters(max_virtual_voices,
			max_voice_limit_groups, max_categories, 
			max_sequence_events_per_frame, max_beatsync_callbacks_per_frame,
			num_standard_memory_voices, num_standard_streaming_voices,
			num_hca_mx_memory_voices, num_hca_mx_streaming_voices,
			output_sampling_rate, num_asr_output_channels,
			uses_in_game_preview, server_frequency,
			max_parameter_blocks, categories_per_playback,
			num_buses, vr_mode,
			IntPtr.Zero);

		CriAtomPlugin.isConfigured = true;
	}

	public static void SetConfigAdditionalParameters_PC(long buffering_time_pc)
	{
		criAtomUnity_SetConfigAdditionalParameters_PC(buffering_time_pc);
	}

	public static void SetConfigAdditionalParameters_IOS(uint buffering_time_ios, bool override_ipod_music_ios)
	{
		criAtomUnity_SetConfigAdditionalParameters_IOS(buffering_time_ios, override_ipod_music_ios);
	}

	public static void SetConfigAdditionalParameters_ANDROID(int num_low_delay_memory_voices, int num_low_delay_streaming_voices,
															 int sound_buffering_time,		  int sound_start_buffering_time,
                                                             IntPtr android_context)
	{
		criAtomUnity_SetConfigAdditionalParameters_ANDROID(num_low_delay_memory_voices, num_low_delay_streaming_voices,
														   sound_buffering_time,		sound_start_buffering_time,
														   android_context);
	}

	public static void SetMaxSamplingRateForStandardVoicePool(int sampling_rate_for_memory, int sampling_rate_for_streaming)
	{
		criAtomUnity_SetMaxSamplingRateForStandardVoicePool(sampling_rate_for_memory, sampling_rate_for_streaming);
	}

	public static int GetRequiredMaxVirtualVoices(CriAtomConfig atomConfig)
	{
		/* バーチャルボイスは、全ボイスプールのボイスの合計値よりも多くなくてはならない */
		int requiredVirtualVoices = 0;

		requiredVirtualVoices += atomConfig.standardVoicePoolConfig.memoryVoices;
		requiredVirtualVoices += atomConfig.standardVoicePoolConfig.streamingVoices;
		requiredVirtualVoices += atomConfig.hcaMxVoicePoolConfig.memoryVoices;
		requiredVirtualVoices += atomConfig.hcaMxVoicePoolConfig.streamingVoices;

		#if UNITY_ANDROID
		requiredVirtualVoices += atomConfig.androidLowLatencyStandardVoicePoolConfig.memoryVoices;
		requiredVirtualVoices += atomConfig.androidLowLatencyStandardVoicePoolConfig.streamingVoices;
		#endif

		return requiredVirtualVoices;
	}

	public static void InitializeLibrary()
	{
		/* 初期化カウンタの更新 */
		CriAtomPlugin.initializationCount++;
		if (CriAtomPlugin.initializationCount != 1) {
			return;
		}

		/* シーン実行前に初期化済みの場合は終了させる */
		if (CriAtomPlugin.IsLibraryInitialized() == true) {
			CriAtomPlugin.FinalizeLibrary();
			CriAtomPlugin.initializationCount = 1;
		}

		/* 初期化パラメータが設定済みかどうかを確認 */
		if (CriAtomPlugin.isConfigured == false) {
			Debug.Log("[CRIWARE] Atom initialization parameters are not configured. "
				+ "Initializes Atom by default parameters.");
		}

		/* 依存ライブラリの初期化 */
		CriFsPlugin.InitializeLibrary();

		/* ライブラリの初期化 */
		CriAtomPlugin.criAtomUnity_Initialize();
	
		/* CriAtomServerのインスタンスを生成 */
		#if UNITY_EDITOR
		/* ゲームプレビュー時のみ生成する */
		if (UnityEngine.Application.isPlaying) {
			CriAtomServer.CreateInstance();
		}
		#else
		CriAtomServer.CreateInstance();
		#endif

		/* CriAtomListener の共有ネイティブリスナーを生成 */
		CriAtomListener.CreateSharedNativeListener();
	}

    public static bool IsLibraryInitialized()
    {
        /* ライブラリが初期化済みかチェック */
        return criAtomUnity_IsInitialized();
    }

	public static void FinalizeLibrary()
	{
		/* 初期化カウンタの更新 */
		CriAtomPlugin.initializationCount--;
		if (CriAtomPlugin.initializationCount < 0) {
			CriAtomPlugin.initializationCount = 0;
			if (CriAtomPlugin.IsLibraryInitialized() == false) {
				return;
			}
		}
		if (CriAtomPlugin.initializationCount != 0) {
			return;
		}

		/* CriAtomListener の共有ネイティブリスナーを破棄 */
		CriAtomListener.DestroySharedNativeListener();

		/* CriAtomServerのインスタンスを破棄 */
		CriAtomServer.DestroyInstance();

		/* 未破棄のDisposableを破棄 */
		CriDisposableObjectManager.CallOnModuleFinalization(CriDisposableObjectManager.ModuleType.Atom);

        /* ユーザエフェクトインタフェースのリストをクリア */
        if (effectInterfaceList != null) {
            effectInterfaceList.Clear();
            effectInterfaceList = null;
        }

		/* ライブラリの終了 */
		CriAtomPlugin.criAtomUnity_Finalize();

		/* 依存ライブラリの終了 */
		CriFsPlugin.FinalizeLibrary();
	}

	public static void Pause(bool pause)
	{
		if (isInitialized) {
			criAtomUnity_Pause(pause);
		}
	}

	#if !UNITY_EDITOR && UNITY_IOS
	public static void CallOnApplicationResume_IOS()
	{
		criAtomUnity_SleepToDelay_IOS(100);
	}
	#endif

	private static bool isConfigured = false;
	private static float timeSinceStartup = Time.realtimeSinceStartup;
	private static CriWare.CpuUsage cpuUsage;
	public static CriWare.CpuUsage GetCpuUsage()
	{
		float currentTimeSinceStartup = Time.realtimeSinceStartup;
		if (currentTimeSinceStartup - timeSinceStartup > 1.0f) {
			CriAtomEx.PerformanceInfo info;
			CriAtomEx.GetPerformanceInfo(out info);

			cpuUsage.last = info.lastServerTime * 100.0f / info.averageServerInterval;
			cpuUsage.average = info.averageServerTime * 100.0f / info.averageServerInterval;
			cpuUsage.peak = info.maxServerTime * 100.0f / info.averageServerInterval;

			CriAtomEx.ResetPerformanceMonitor();
			timeSinceStartup = currentTimeSinceStartup;
		}
		return cpuUsage;
	}

	private static int CRIATOMUNITY_PARAMETER_ID_LOOP_COUNT = 0;
	private static ushort CRIATOMPARAMETER2_ID_INVALID = ushort.MaxValue;

	public static ushort GetLoopCountParameterId()
	{
		ushort ret = criAtomUnity_GetNativeParameterId(CRIATOMUNITY_PARAMETER_ID_LOOP_COUNT);
		if (ret == CRIATOMPARAMETER2_ID_INVALID) {
			throw new Exception("GetNativeParameterId failed.");
		}
		return ret;
	}

	#region DLL Import
	#if !CRIWARE_ENABLE_HEADLESS_MODE
    [DllImport(CriWare.pluginName, CallingConvention = CriWare.pluginCallingConvention)]
    private static extern void criAtomUnity_SetConfigParameters(int max_virtual_voices,
        int max_voice_limit_groups, int max_categories,
		int max_sequence_events_per_frame, int max_beatsync_callbacks_per_frame,
        int num_standard_memory_voices, int num_standard_streaming_voices,
        int num_hca_mx_memory_voices, int num_hca_mx_streaming_voices,
        int output_sampling_rate, int num_asr_output_channels,
        bool uses_in_game_preview, float server_frequency,
        int max_parameter_blocks, int categories_per_playback,
        int num_buses, bool use_ambisonics,
        IntPtr spatializer_core_interface);

	[DllImport(CriWare.pluginName, CallingConvention = CriWare.pluginCallingConvention)]
	private static extern void criAtomUnity_SetConfigAdditionalParameters_PC(long buffering_time_pc);

	[DllImport(CriWare.pluginName, CallingConvention = CriWare.pluginCallingConvention)]
	private static extern void criAtomUnity_SetConfigAdditionalParameters_IOS(uint buffering_time_ios, bool override_ipod_music_ios);

	[DllImport(CriWare.pluginName, CallingConvention = CriWare.pluginCallingConvention)]
	private static extern void criAtomUnity_SetConfigAdditionalParameters_ANDROID(int num_low_delay_memory_voices, int num_low_delay_streaming_voices,
																				  int sound_buffering_time,		   int sound_start_buffering_time,
																				  IntPtr android_context);

	[DllImport(CriWare.pluginName, CallingConvention = CriWare.pluginCallingConvention)]
	private static extern void criAtomUnity_Initialize();

    [DllImport(CriWare.pluginName, CallingConvention = CriWare.pluginCallingConvention)]
    public static extern bool criAtomUnity_IsInitialized();

	[DllImport(CriWare.pluginName, CallingConvention = CriWare.pluginCallingConvention)]
	private static extern void criAtomUnity_Finalize();

	[DllImport(CriWare.pluginName, CallingConvention = CriWare.pluginCallingConvention)]
	private static extern void criAtomUnity_Pause(bool pause);

	[DllImport(CriWare.pluginName, CallingConvention = CriWare.pluginCallingConvention)]
	public static extern uint criAtomUnity_GetAllocatedHeapSize();

	[DllImport(CriWare.pluginName, CallingConvention = CriWare.pluginCallingConvention)]
	public static extern void criAtomUnity_ControlDataCompatibility(int code);

	[DllImport(CriWare.pluginName, CallingConvention = CriWare.pluginCallingConvention)]
	public static extern void criAtomUnitySequencer_SetEventCallback(IntPtr cbfunc, string separator_string);

	[DllImport(CriWare.pluginName, CallingConvention = CriWare.pluginCallingConvention)]
	public static extern void criAtomUnitySequencer_ExecuteQueuedEventCallbacks();

	[DllImport(CriWare.pluginName, CallingConvention = CriWare.pluginCallingConvention)]
	public static extern void criAtomUnity_SetBeatSyncCallback(IntPtr cbfunc);

	[DllImport(CriWare.pluginName, CallingConvention = CriWare.pluginCallingConvention)]
	public static extern void criAtomUnity_ExecuteQueuedBeatSyncCallbacks();

	[DllImport(CriWare.pluginName, CallingConvention = CriWare.pluginCallingConvention)]
	private static extern void criAtomUnity_SetMaxSamplingRateForStandardVoicePool(int sampling_rate_for_memory, int sampling_rate_for_streaming);

	[DllImport(CriWare.pluginName, CallingConvention = CriWare.pluginCallingConvention)]
	public static extern void criAtomUnity_BeginLeCompatibleMode();

	[DllImport(CriWare.pluginName, CallingConvention = CriWare.pluginCallingConvention)]
	public static extern void criAtomUnity_EndLeCompatibleMode();

	[DllImport(CriWare.pluginName, CallingConvention = CriWare.pluginCallingConvention)]
	public static extern ushort criAtomUnity_GetNativeParameterId(int id);

	#if !UNITY_EDITOR && UNITY_IOS
	[DllImport(CriWare.pluginName, CallingConvention = CriWare.pluginCallingConvention)]
	public static extern void criAtomUnity_SleepToDelay_IOS(int milliseconds);
	#endif
	#else
	private static void criAtomUnity_SetConfigParameters(int max_virtual_voices,
		int max_voice_limit_groups, int max_categories,
		int max_sequence_events_per_frame, int max_beatsync_callbacks_per_frame,
		int num_standard_memory_voices, int num_standard_streaming_voices,
		int num_hca_mx_memory_voices, int num_hca_mx_streaming_voices,
		int output_sampling_rate, int num_asr_output_channels,
		bool uses_in_game_preview, float server_frequency,
		int max_parameter_blocks, int categories_per_playback,
		int num_buses, bool use_ambisonics,
		IntPtr spatializer_core_interface) { }
	private static void criAtomUnity_SetConfigAdditionalParameters_PC(long buffering_time_pc) { }
	private static void criAtomUnity_SetConfigAdditionalParameters_IOS(uint buffering_time_ios, bool override_ipod_music_ios) { }
	private static void criAtomUnity_SetConfigAdditionalParameters_ANDROID(int num_low_delay_memory_voices, int num_low_delay_streaming_voices,
																			int sound_buffering_time,		   int sound_start_buffering_time,
																			IntPtr android_context) { }
	private static bool _dummyInitialized = false;
	private static void criAtomUnity_Initialize() { _dummyInitialized = true; }
	public static bool criAtomUnity_IsInitialized() { return _dummyInitialized; }
	private static void criAtomUnity_Finalize() { _dummyInitialized = false; }
	private static void criAtomUnity_Pause(bool pause) { }
	public static uint criAtomUnity_GetAllocatedHeapSize() { return 0; }
	public static void criAtomUnity_ControlDataCompatibility(int code) { }
	public static void criAtomUnitySequencer_SetEventCallback(IntPtr cbfunc, string separator_string) { }
	public static void criAtomUnitySequencer_ExecuteQueuedEventCallbacks() { }
	public static void criAtomUnity_SetBeatSyncCallback(IntPtr cbfunc) { }
	public static void criAtomUnity_ExecuteQueuedBeatSyncCallbacks() { }
	private static void criAtomUnity_SetMaxSamplingRateForStandardVoicePool(int sampling_rate_for_memory, int sampling_rate_for_streaming) { }
	public static void criAtomUnity_BeginLeCompatibleMode() { }
	public static void criAtomUnity_EndLeCompatibleMode() { }
	public static ushort criAtomUnity_GetNativeParameterId(int id) { return 0; }
#if !UNITY_EDITOR && UNITY_IOS
	public static void criAtomUnity_SleepToDelay_IOS(int milliseconds) { }
#endif
	#endif
	#endregion

	#endregion
}

[Serializable]
public class CriAtomCueSheet
{
	public string name = "";
	public string acbFile = "";
	public string awbFile = "";
	public CriAtomExAcb acb;
	public CriAtomExAcbLoader.Status loaderStatus = CriAtomExAcbLoader.Status.Stop;
	public bool IsLoading { get { return loaderStatus == CriAtomExAcbLoader.Status.Loading; } }
    public bool IsError { get { return (loaderStatus == CriAtomExAcbLoader.Status.Error) || (!IsLoading && acb == null); } }
}

/// \addtogroup CRIATOM_UNITY_COMPONENT
/// @{
/**
 * <summary>サウンド再生全体を制御するためのコンポーネントです。</summary>
 * \par 説明:
 * 各シーンにひとつ用意しておく必要があります。<br/>
 * UnityEditor上でCRI Atomウィンドウを使用して CriAtomSource を作成した場合、
 * 「CRIWARE」という名前を持つオブジェクトとして自動的に作成されます。通常はユーザーが作成する必要はありません。
 */
[AddComponentMenu("CRIWARE/CRI Atom")]
public class CriAtom : MonoBehaviour
{

	/* @cond DOXYGEN_IGNORE */
	public string acfFile = "";
	private bool acfIsLoading = false;
#if CRIWARE_FORCE_LOAD_ASYNC
	private byte[] acfData = null;
#endif
	public CriAtomCueSheet[] cueSheets = {};
	public string dspBusSetting = "";
	public bool dontDestroyOnLoad = false;
#if CRIWARE_SUPPORT_NATIVE_CALLBACK
	private static CriAtomExSequencer.EventCbFunc eventUserCbFunc = null;
	private static CriAtomExBeatSync.CbFunc beatsyncUserCbFunc = null;
#endif
	private static CriAtom instance {
		get; set;
	}

	/* @endcond */

	#region Functions

	/**
	 * <summary>DSPバス設定のアタッチ</summary>
	 * <param name="settingName">DSPバス設定の名前</param>
	 * \par 説明:
	 * DSPバス設定からDSPバスを構築してサウンドレンダラにアタッチします。<br/>
	 * 現在設定中のDSPバス設定を切り替える場合は、一度デタッチしてから再アタッチしてください。
	 * <br/>
	 * \attention
	 * 本関数は完了復帰型の関数です。<br/>
	 * 本関数を実行すると、しばらくの間Atomライブラリのサーバ処理がブロックされます。<br/>
	 * 音声再生中に本関数を実行すると、音途切れ等の不具合が発生する可能性があるため、
	 * 本関数の呼び出しはシーンの切り替わり等、負荷変動を許容できるタイミングで行ってください。<br/>
	 * \sa CriAtom::DetachDspBusSetting
	 */
	public static void AttachDspBusSetting(string settingName)
	{
		CriAtom.instance.dspBusSetting = settingName;
		if (!String.IsNullOrEmpty(settingName)) {
			CriAtomEx.AttachDspBusSetting(settingName);
		} else {
			CriAtomEx.DetachDspBusSetting();
		}
	}

	/**
	 * <summary>DSPバス設定のデタッチ</summary>
	 * \par 説明:
	 * DSPバス設定をデタッチします。<br/>
	 * <br/>
	 * \attention
	 * 本関数は完了復帰型の関数です。<br/>
	 * 本関数を実行すると、しばらくの間Atomライブラリのサーバ処理がブロックされます。<br/>
	 * 音声再生中に本関数を実行すると、音途切れ等の不具合が発生する可能性があるため、
	 * 本関数の呼び出しはシーンの切り替わり等、負荷変動を許容できるタイミングで行ってください。<br/>
	 * \sa CriAtom::DetachDspBusSetting
	 */
	public static void DetachDspBusSetting()
	{
		CriAtom.instance.dspBusSetting = "";
		CriAtomEx.DetachDspBusSetting();
	}

	/**
	 * <summary>キューシートの取得</summary>
	 * <param name="name">キューシート名</param>
	 * <returns>キューシートオブジェクト</returns>
	 * \par 説明:
	 * 引数に指定したキューシート名を元に登録済みのキューシートオブジェクトを取得します。<br/>
	 */
	public static CriAtomCueSheet GetCueSheet(string name)
	{
		return CriAtom.instance.GetCueSheetInternal(name);
	}

	/**
	 * <summary>キューシートの追加</summary>
	 * <param name="name">キューシート名</param>
	 * <param name="acbFile">ACBファイルパス</param>
	 * <param name="awbFile">AWBファイルパス</param>
	 * <param name="binder">バインダオブジェクト(オプション)</param>
	 * <returns>キューシートオブジェクト</returns>
	 * \par 説明:
	 * 引数に指定したファイルパス情報を元にキューシートの追加を行います。<br/>
	 * 同時に複数のキューシートを登録することが可能です。<br/>
	 * <br/>
	 * 各ファイルパスに対して相対パスを指定した場合は StreamingAssets フォルダからの相対でファイルをロードします。<br/>
	 * 絶対パス、あるいはURLを指定した場合には指定したパスでファイルをロードします。<br/>
	 * <br/>
	 * CPKファイルにパッキングされたACBファイルとAWBファイルからキューシートを追加する場合は、
	 * binder引数にCPKをバインドしたバインダを指定してください。<br/>
	 * なお、バインダ機能はADX2LEではご利用になれません。<br/>
	 */
	public static CriAtomCueSheet AddCueSheet(string name, string acbFile, string awbFile, CriFsBinder binder = null)
	{
	#if CRIWARE_FORCE_LOAD_ASYNC
		return CriAtom.AddCueSheetAsync(name, acbFile, awbFile, binder);
	#else
		CriAtomCueSheet cueSheet = CriAtom.instance.AddCueSheetInternal(name, acbFile, awbFile, binder);
		if (Application.isPlaying) {
			cueSheet.acb = CriAtom.instance.LoadAcbFile(binder, acbFile, awbFile);
		}
		return cueSheet;
	#endif
	}

	/**
	 * <summary>非同期でのキューシートの追加</summary>
	 * <param name="name">キューシート名</param>
	 * <param name="acbFile">ACBファイルパス</param>
	 * <param name="awbFile">AWBファイルパス</param>
	 * <param name="binder">バインダオブジェクト(オプション)</param>
	 * <param name="loadAwbOnMemory">AWBファイルをメモリ上にロードするか(オプション)</param>
	 * <returns>キューシートオブジェクト</returns>
	 * \par 説明:
	 * 引数に指定したファイルパス情報を元に、非同期でキューシートの追加を行います。<br/>
	 * 同時に複数のキューシートを登録することが可能です。<br/>
	 * <br/>
	 * 各ファイルパスに対して相対パスを指定した場合は StreamingAssets フォルダからの相対でファイルをロードします。<br/>
	 * 絶対パス、あるいはURLを指定した場合には指定したパスでファイルをロードします。<br/>
	 * <br/>
	 * CPKファイルにパッキングされたACBファイルとAWBファイルからキューシートを追加する場合は、
	 * binder引数にCPKをバインドしたバインダを指定してください。<br/>
	 * なお、バインダ機能はADX2LEではご利用になれません。<br/>
	 * <br/>
	 * 戻り値のキューシートオブジェクトの CriAtomCueSheet::isLoading メンバが true を返す間はロード中です。<br/>
	 * 必ず false を返すのを確認してからキューの再生等を行うようにしてください。<br/>
	 * <br/>
	 * loadAwbOnMemory が false の場合、AWBファイルのヘッダ部分のみをメモリ上にロードしストリーム再生を行います。<br/>
	 * loadAwbOnMemory を true に設定すると、AWBファイル全体をメモリ上にロードするため実質メモリ再生になります。<br/>
	 */
	public static CriAtomCueSheet AddCueSheetAsync(string name, string acbFile, string awbFile, CriFsBinder binder = null, bool loadAwbOnMemory = false)
	{
	#if UNITY_EDITOR && UNITY_WEBGL
		loadAwbOnMemory = true;
	#endif
		CriAtomCueSheet cueSheet = CriAtom.instance.AddCueSheetInternal(name, acbFile, awbFile, binder);
		if (Application.isPlaying) {
			CriAtom.instance.LoadAcbFileAsync(cueSheet, binder, acbFile, awbFile, loadAwbOnMemory);
		}
		return cueSheet;
	}

	/**
	 * <summary>キューシートの追加（メモリからの読み込み）</summary>
	 * <param name="name">キューシート名</param>
	 * <param name="acbData">ACBデータ</param>
	 * <param name="awbFile">AWBファイルパス</param>
	 * <param name="awbBinder">AWB用バインダオブジェクト(オプション)</param>
	 * <returns>キューシートオブジェクト</returns>
	 * \par 説明:
	 * 引数に指定したデータとファイルパス情報からキューシートの追加を行います。<br/>
	 * 同時に複数のキューシートを登録することが可能です。<br/>
	 * <br/>
	 * ファイルパスに対して相対パスを指定した場合は StreamingAssets フォルダからの相対でファイルをロードします。<br/>
	 * 絶対パス、あるいはURLを指定した場合には指定したパスでファイルをロードします。<br/>
	 * <br/>
	 * CPKファイルにパッキングされたAWBファイルからキューシートを追加する場合は、
	 * awbBinder引数にCPKをバインドしたバインダを指定してください。<br/>
	 * なお、バインダ機能はADX2LEではご利用になれません。<br/>
	 */
	public static CriAtomCueSheet AddCueSheet(string name, byte[] acbData, string awbFile, CriFsBinder awbBinder = null)
	{
	#if CRIWARE_FORCE_LOAD_ASYNC
		return CriAtom.AddCueSheetAsync(name, acbData, awbFile, awbBinder);
	#else
		CriAtomCueSheet cueSheet = CriAtom.instance.AddCueSheetInternal(name, "", awbFile, awbBinder);
		if (Application.isPlaying) {
			cueSheet.acb = CriAtom.instance.LoadAcbData(acbData, awbBinder, awbFile);
		}
		return cueSheet;
	#endif
	}

	/**
	 * <summary>非同期でのキューシートの追加（メモリからの読み込み）</summary>
	 * <param name="name">キューシート名</param>
	 * <param name="acbData">ACBデータ</param>
	 * <param name="awbFile">AWBファイルパス</param>
	 * <param name="awbBinder">AWB用バインダオブジェクト(オプション)</param>
 	 * <param name="loadAwbOnMemory">AWBファイルをメモリ上にロードするか(オプション)</param>
	 * <returns>キューシートオブジェクト</returns>
	 * \par 説明:
	 * 引数に指定したデータとファイルパス情報からキューシートの追加を行います。<br/>
	 * 同時に複数のキューシートを登録することが可能です。<br/>
	 * <br/>
	 * ファイルパスに対して相対パスを指定した場合は StreamingAssets フォルダからの相対でファイルをロードします。<br/>
	 * 絶対パス、あるいはURLを指定した場合には指定したパスでファイルをロードします。<br/>
	 * <br/>
	 * CPKファイルにパッキングされたAWBファイルからキューシートを追加する場合は、
	 * awbBinder引数にCPKをバインドしたバインダを指定してください。<br/>
	 * なお、バインダ機能はADX2LEではご利用になれません。<br/>
	 * <br/>
	 * 戻り値のキューシートオブジェクトの CriAtomCueSheet::isLoading メンバが true を返す間はロード中です。<br/>
	 * 必ず false を返すのを確認してからキューの再生等を行うようにしてください。<br/>
	 * <br/>
	 * loadAwbOnMemory が false の場合、AWBファイルのヘッダ部分のみをメモリ上にロードしストリーム再生を行います。<br/>
	 * loadAwbOnMemory を true に設定すると、AWBファイル全体をメモリ上にロードするため実質メモリ再生になります。<br/>
	 */
	public static CriAtomCueSheet AddCueSheetAsync(string name, byte[] acbData, string awbFile, CriFsBinder awbBinder = null, bool loadAwbOnMemory = false)
	{
	#if UNITY_EDITOR && UNITY_WEBGL
		loadAwbOnMemory = true;
	#endif
		CriAtomCueSheet cueSheet = CriAtom.instance.AddCueSheetInternal(name, "", awbFile, awbBinder);
		if (Application.isPlaying) {
			CriAtom.instance.LoadAcbDataAsync(cueSheet, acbData, awbBinder, awbFile, loadAwbOnMemory);
		}
		return cueSheet;
	}

	/**
	 * <summary>キューシートの削除</summary>
	 * <param name="name">キューシート名</param>
	 * \par 説明:
	 * 追加済みのキューシートを削除します。<br/>
	 */
	public static void RemoveCueSheet(string name)
	{
        if (CriAtom.instance == null) {
            return;
        }
		CriAtom.instance.RemoveCueSheetInternal(name);
	}

	/**
	 * <summary>キューシートのロード完了チェック</summary>
	 * \par 説明:
	 * 全てのキューシートのロード完了をチェックします。<br/>
	 */
	public static bool CueSheetsAreLoading {
		get {
			if (CriAtom.instance == null) {
				return false;
			}
			foreach (var cueSheet in CriAtom.instance.cueSheets) {
				if (cueSheet.IsLoading) {
					return true;
				}
			}
			return false;
		}
	}

	/**
	 * <summary>ACBオブジェクトの取得</summary>
	 * <param name="cueSheetName">キューシート名</param>
	 * <returns>ACBオブジェクト</returns>
	 * \par 説明:
	 * 指定したキューシートに対応するACBオブジェクトを取得します。<br/>
	 */
	public static CriAtomExAcb GetAcb(string cueSheetName)
	{
		foreach (var cueSheet in CriAtom.instance.cueSheets) {
			if (cueSheetName == cueSheet.name) {
				return cueSheet.acb;
			}
		}
		Debug.LogWarning(cueSheetName + " is not loaded.");
		return null;
	}

	/**
	 * <summary>カテゴリ名指定でカテゴリボリュームを設定します。</summary>
	 * <param name="name">カテゴリ名</param>
	 * <param name="volume">ボリューム</param>
	 */
	public static void SetCategoryVolume(string name, float volume)
	{
		CriAtomExCategory.SetVolume(name, volume);
	}

	/**
	 * <summary>カテゴリID指定でカテゴリボリュームを設定します。</summary>
	 * <param name="id">カテゴリID</param>
	 * <param name="volume">ボリューム</param>
	 */
	public static void SetCategoryVolume(int id, float volume)
	{
		CriAtomExCategory.SetVolume(id, volume);
	}

	/**
	 * <summary>カテゴリ名指定でカテゴリボリュームを取得します。</summary>
	 * <param name="name">カテゴリ名</param>
	 * <returns>ボリューム</returns>
	 */
	public static float GetCategoryVolume(string name)
	{
		return CriAtomExCategory.GetVolume(name);
	}
	/**
	 * <summary>カテゴリID指定でカテゴリボリュームを取得します。</summary>
	 * <param name="id">カテゴリID</param>
	 * <returns>ボリューム</returns>
	 */
	public static float GetCategoryVolume(int id)
	{
		return CriAtomExCategory.GetVolume(id);
	}

	/**
	 * <summary>バス情報取得を有効にします。</summary>
	 * <param name="busName">DSPバス名</param>
	 * <param name="sw">true: 取得を有効にする。 false: 取得を無効にする</param>
	 */
	public static void SetBusAnalyzer(string busName, bool sw)
	{
		if (sw) {
			CriAtomExAsr.AttachBusAnalyzer(busName, 50, 1000);
		} else {
			CriAtomExAsr.DetachBusAnalyzer(busName);
		}
	}

	/**
	 * <summary>全てのバス情報取得を有効にします。</summary>
	 * <param name="sw">true: 取得を有効にする。 false: 取得を無効にする</param>
	 */
	public static void SetBusAnalyzer(bool sw)
	{
		if (sw) {
			CriAtomExAsr.AttachBusAnalyzer(50, 1000);
		} else {
			CriAtomExAsr.DetachBusAnalyzer();
		}
	}

	/**
	 * <summary>バス情報を取得します。</summary>
	 * <param name="busName">DSPバス名</param>
	 * <returns>DSPバス情報</returns>
	 */
	public static CriAtomExAsr.BusAnalyzerInfo GetBusAnalyzerInfo(string busName)
	{
		CriAtomExAsr.BusAnalyzerInfo info;
		CriAtomExAsr.GetBusAnalyzerInfo(busName, out info);
		return info;
	}

	[System.Obsolete("Use CriAtom.GetBusAnalyzerInfo(string busName)")]
	public static CriAtomExAsr.BusAnalyzerInfo GetBusAnalyzerInfo(int busId)
	{
		CriAtomExAsr.BusAnalyzerInfo info;
		CriAtomExAsr.GetBusAnalyzerInfo(busId, out info);
		return info;
	}

	#endregion // Functions

	/* @cond DOXYGEN_IGNORE */
	#region Functions for internal use

	public void Setup()
	{
		if (CriAtom.instance != null && CriAtom.instance != this) {
			var obj = CriAtom.instance.gameObject;
			CriAtom.instance.Shutdown();
			CriAtomEx.UnregisterAcf();
			GameObject.Destroy(obj);
		}

		CriAtom.instance = this;

		CriAtomPlugin.InitializeLibrary();

		if (!String.IsNullOrEmpty(this.acfFile)) {
			string acfPath = Path.Combine(CriWare.streamingAssetsPath, this.acfFile);
			CriAtomEx.RegisterAcf(null, acfPath);
		}

		if (!String.IsNullOrEmpty(dspBusSetting)) {
			AttachDspBusSetting(dspBusSetting);
		}

		foreach (var cueSheet in this.cueSheets) {
			cueSheet.acb = this.LoadAcbFile(null, cueSheet.acbFile, cueSheet.awbFile);
		}

		if (this.dontDestroyOnLoad) {
			GameObject.DontDestroyOnLoad(this.gameObject);
		}
	}

	public void Shutdown()
	{
		foreach (var cueSheet in this.cueSheets) {
			if (cueSheet.acb != null) {
				cueSheet.acb.Dispose();
				cueSheet.acb = null;
			}
		}
		CriAtomPlugin.FinalizeLibrary();
		if (CriAtom.instance == this) {
			CriAtom.instance = null;
		}
	}

	private void Awake()
	{
		if (CriAtom.instance != null && CriAtom.instance != this) {
			if (CriAtom.instance.acfFile != this.acfFile) {
				var obj = CriAtom.instance.gameObject;
				CriAtom.instance.Shutdown();
				CriAtomEx.UnregisterAcf();
				GameObject.Destroy(obj);
				return;
			}
			if (CriAtom.instance.dspBusSetting != this.dspBusSetting) {
				CriAtom.AttachDspBusSetting(this.dspBusSetting);
			}
			CriAtom.instance.MargeCueSheet(this.cueSheets, this.dontRemoveExistsCueSheet);
			GameObject.Destroy(this.gameObject);
		}
	}

	private void OnEnable()
	{
	#if UNITY_EDITOR
		if (CriAtomPlugin.previewCallback != null) {
			CriAtomPlugin.previewCallback();
		}
	#endif
		if (CriAtom.instance != null) {
			return;
		}

	#if CRIWARE_FORCE_LOAD_ASYNC
		this.SetupAsync();
	#else
		this.Setup();
	#endif
	}

	private void OnDestroy()
	{
		if (this != CriAtom.instance) {
			return;
		}
		this.Shutdown();
	}

	private void Update()
	{
		CriAtomPlugin.criAtomUnitySequencer_ExecuteQueuedEventCallbacks();
		CriAtomPlugin.criAtomUnity_ExecuteQueuedBeatSyncCallbacks();
	}

	public CriAtomCueSheet GetCueSheetInternal(string name)
	{
		for (int i = 0; i < this.cueSheets.Length; i++) {
			CriAtomCueSheet cueSheet = this.cueSheets[i];
			if (cueSheet.name == name) {
				return cueSheet;
			}
		}
		return null;
	}

	public CriAtomCueSheet AddCueSheetInternal(string name, string acbFile, string awbFile, CriFsBinder binder)
	{
		var cueSheets = new CriAtomCueSheet[this.cueSheets.Length + 1];
		this.cueSheets.CopyTo(cueSheets, 0);
		this.cueSheets = cueSheets;

		var cueSheet = new CriAtomCueSheet();
		this.cueSheets[this.cueSheets.Length - 1] = cueSheet;
		if (String.IsNullOrEmpty(name)) {
			cueSheet.name = Path.GetFileNameWithoutExtension(acbFile);
		} else {
			cueSheet.name = name;
		}
		cueSheet.acbFile = acbFile;
		cueSheet.awbFile = awbFile;
		return cueSheet;
	}

	public void RemoveCueSheetInternal(string name)
	{
		int index = -1;
		for (int i = 0; i < this.cueSheets.Length; i++) {
			if (name == this.cueSheets[i].name) {
				index = i;
			}
		}
		if (index < 0) {
			return;
		}

		var cueSheet = this.cueSheets[index];
		if (cueSheet.acb != null) {
			cueSheet.acb.Dispose();
			cueSheet.acb = null;
		}

		var cueSheets = new CriAtomCueSheet[this.cueSheets.Length - 1];
		Array.Copy(this.cueSheets, 0, cueSheets, 0, index);
		Array.Copy(this.cueSheets, index + 1, cueSheets, index, this.cueSheets.Length - index - 1);
		this.cueSheets = cueSheets;
	}

	public bool dontRemoveExistsCueSheet = false;

	/*
	 * newDontRemoveExistsCueSheet == false の場合、古いキューシートを登録解除して、新しいキューシートの登録を行う。
	 * ただし同じキューシートの再登録は回避する
	 */
	private void MargeCueSheet(CriAtomCueSheet[] newCueSheets, bool newDontRemoveExistsCueSheet)
	{
		if (!newDontRemoveExistsCueSheet) {
			for (int i = 0; i < this.cueSheets.Length; ) {
				int index = Array.FindIndex(newCueSheets, sheet => sheet.name == this.cueSheets[i].name);
				if (index < 0) {
					CriAtom.RemoveCueSheet(this.cueSheets[i].name);
				} else {
					i++;
				}
			}
		}

		foreach (var sheet1 in newCueSheets) {
			var sheet2 = this.GetCueSheetInternal(sheet1.name);
			if (sheet2 == null) {
				CriAtom.AddCueSheet(null, sheet1.acbFile, sheet1.awbFile, null);
			}
		}
	}

	private CriAtomExAcb LoadAcbFile(CriFsBinder binder, string acbFile, string awbFile)
	{
		if (String.IsNullOrEmpty(acbFile)) {
			return null;
		}

		string acbPath = acbFile;
		if ((binder == null) && CriWare.IsStreamingAssetsPath(acbPath)) {
			acbPath = Path.Combine(CriWare.streamingAssetsPath, acbPath);
		}

		string awbPath = awbFile;
		if (!String.IsNullOrEmpty(awbPath)) {
			if ((binder == null) && CriWare.IsStreamingAssetsPath(awbPath)) {
				awbPath = Path.Combine(CriWare.streamingAssetsPath, awbPath);
			}
		}

		return CriAtomExAcb.LoadAcbFile(binder, acbPath, awbPath);
	}

	private CriAtomExAcb LoadAcbData(byte[] acbData, CriFsBinder binder, string awbFile)
	{
		if (acbData == null) {
			return null;
		}

		string awbPath = awbFile;
		if (!String.IsNullOrEmpty(awbPath)) {
			if ((binder == null) && CriWare.IsStreamingAssetsPath(awbPath)) {
				awbPath = Path.Combine(CriWare.streamingAssetsPath, awbPath);
			}
		}

		return CriAtomExAcb.LoadAcbData(acbData, binder, awbPath);
	}

#if CRIWARE_FORCE_LOAD_ASYNC
	private void SetupAsync()
	{
		CriAtom.instance = this;

		CriAtomPlugin.InitializeLibrary();

		if (this.dontDestroyOnLoad) {
			GameObject.DontDestroyOnLoad(this.gameObject);
		}

		if (!String.IsNullOrEmpty(this.acfFile)) {
			this.acfIsLoading = true;
			StartCoroutine(RegisterAcfAsync(this.acfFile, this.dspBusSetting));
		}

		foreach (var cueSheet in this.cueSheets) {
			#if UNITY_EDITOR
			bool loadAwbOnMemory = true;
			#else
			bool loadAwbOnMemory = false;
			#endif
			StartCoroutine(LoadAcbFileCoroutine(cueSheet, null, cueSheet.acbFile, cueSheet.awbFile, loadAwbOnMemory));
		}
	}

	private IEnumerator RegisterAcfAsync(string acfFile, string dspBusSetting)
	{
	#if UNITY_EDITOR
		string acfPath = "file://" + CriWare.streamingAssetsPath + "/" + acfFile;
	#else
		string acfPath = CriWare.streamingAssetsPath + "/" + acfFile;
	#endif
		using (var req = new WWW(acfPath)) {
			yield return req;
			this.acfData = req.bytes;
		}
		CriAtomEx.RegisterAcf(this.acfData);

		if (!String.IsNullOrEmpty(dspBusSetting)) {
			AttachDspBusSetting(dspBusSetting);
		}
		this.acfIsLoading = false;
	}
#endif


	private void LoadAcbFileAsync(CriAtomCueSheet cueSheet, CriFsBinder binder, string acbFile, string awbFile, bool loadAwbOnMemory)
	{
		if (String.IsNullOrEmpty(acbFile)) {
			return;
		}

		StartCoroutine(LoadAcbFileCoroutine(cueSheet, binder, acbFile, awbFile, loadAwbOnMemory));
	}

	private IEnumerator LoadAcbFileCoroutine(CriAtomCueSheet cueSheet, CriFsBinder binder, string acbPath, string awbPath, bool loadAwbOnMemory)
	{
		cueSheet.loaderStatus = CriAtomExAcbLoader.Status.Loading;

		if ((binder == null) && CriWare.IsStreamingAssetsPath(acbPath)) {
			acbPath = Path.Combine(CriWare.streamingAssetsPath, acbPath);
		}

		if (!String.IsNullOrEmpty(awbPath)) {
			if ((binder == null) && CriWare.IsStreamingAssetsPath(awbPath)) {
				awbPath = Path.Combine(CriWare.streamingAssetsPath, awbPath);
			}
		}

		while (this.acfIsLoading) {
			yield return new WaitForEndOfFrame();
		}

		using (var asyncLoader = CriAtomExAcbLoader.LoadAcbFileAsync(binder, acbPath, awbPath, loadAwbOnMemory)) {
			if (asyncLoader == null) {
				cueSheet.loaderStatus = CriAtomExAcbLoader.Status.Error;
				yield break;
			}

			while (true) {
				var status = asyncLoader.GetStatus();
				cueSheet.loaderStatus = status;
				if (status == CriAtomExAcbLoader.Status.Complete) {
					cueSheet.acb = asyncLoader.MoveAcb();
					break;
				} else if (status == CriAtomExAcbLoader.Status.Error) {
					break;
				}
				yield return new WaitForEndOfFrame();
			}
		}
	}

	private void LoadAcbDataAsync(CriAtomCueSheet cueSheet, byte[] acbData, CriFsBinder awbBinder, string awbFile, bool loadAwbOnMemory)
	{
		StartCoroutine(LoadAcbDataCoroutine(cueSheet, acbData, awbBinder, awbFile, loadAwbOnMemory));
	}

	private IEnumerator LoadAcbDataCoroutine(CriAtomCueSheet cueSheet, byte[] acbData, CriFsBinder awbBinder, string awbPath, bool loadAwbOnMemory)
	{
		cueSheet.loaderStatus = CriAtomExAcbLoader.Status.Loading;

		if (!String.IsNullOrEmpty(awbPath)) {
			if ((awbBinder == null) && CriWare.IsStreamingAssetsPath(awbPath)) {
				awbPath = Path.Combine(CriWare.streamingAssetsPath, awbPath);
			}
		}

		while (this.acfIsLoading) {
			yield return new WaitForEndOfFrame();
		}

		using (var asyncLoader = CriAtomExAcbLoader.LoadAcbDataAsync(acbData, awbBinder, awbPath, loadAwbOnMemory)) {
			if (asyncLoader == null) {
				cueSheet.loaderStatus = CriAtomExAcbLoader.Status.Error;
				yield break;
			}

			while (true) {
				var status = asyncLoader.GetStatus();
				cueSheet.loaderStatus = status;
				if (status == CriAtomExAcbLoader.Status.Complete) {
					cueSheet.acb = asyncLoader.MoveAcb();
					break;
				} else if (status == CriAtomExAcbLoader.Status.Error) {
					break;
				}
				yield return new WaitForEndOfFrame();
			}
		}
	}

#if CRIWARE_SUPPORT_NATIVE_CALLBACK
	[AOT.MonoPInvokeCallback(typeof(CriAtomExSequencer.EventCbFunc))]
	public static void SequenceEventCallbackFromNative(string eventString)
	{
		/* 本関数はアプリケーションメインスレッドの CriAtom.Update から呼び出される */
		if (eventUserCbFunc != null) {
			eventUserCbFunc(eventString);
		}
	}

	[AOT.MonoPInvokeCallback(typeof(CriAtomExBeatSync.CbFunc))]
	public static void BeatSyncCallbackFromNative(ref CriAtomExBeatSync.Info info)
	{
		/* 本関数はアプリケーションメインスレッドの CriAtom.Update から呼び出される */
		if (beatsyncUserCbFunc != null) {
			beatsyncUserCbFunc(ref info);
		}
	}
#endif

	/* プラグイン内部用API */
	public static void SetEventCallback(CriAtomExSequencer.EventCbFunc func, string separator)
	{
#if CRIWARE_SUPPORT_NATIVE_CALLBACK
		/* ネイティブプラグインに関数ポインタを登録 */
		IntPtr ptr = IntPtr.Zero;
		eventUserCbFunc = func;
		if (func != null) {
			CriAtomExSequencer.EventCbFunc delegateObject;
			delegateObject = new CriAtomExSequencer.EventCbFunc(CriAtom.SequenceEventCallbackFromNative);
			ptr = Marshal.GetFunctionPointerForDelegate(delegateObject);
		}
		CriAtomPlugin.criAtomUnitySequencer_SetEventCallback(ptr, separator);
#else
		Debug.LogError("[CRIWARE] Event callback is not supported for this scripting backend.");
#endif
	}

	public static void SetBeatSyncCallback(CriAtomExBeatSync.CbFunc func)
	{
#if CRIWARE_SUPPORT_NATIVE_CALLBACK
		/* ネイティブプラグインに関数ポインタを登録 */
		IntPtr ptr = IntPtr.Zero;
		beatsyncUserCbFunc = func;
		if (func != null) {
			CriAtomExBeatSync.CbFunc delegateObject;
			delegateObject = new CriAtomExBeatSync.CbFunc (CriAtom.BeatSyncCallbackFromNative);
			ptr = Marshal.GetFunctionPointerForDelegate(delegateObject);
		}
		CriAtomPlugin.criAtomUnity_SetBeatSyncCallback(ptr);
#else
        Debug.LogError("[CRIWARE] Beat sync callback is not supported for this scripting backend.");
#endif
	}

	#endregion
	/* @endcond */

}

/// @}
/* end of file */
>>>>>>> sound
