
﻿/****************************************************************************
 *
 * Copyright (c) 2011 CRI Middleware Co., Ltd.
 *
 ****************************************************************************/

#define CRIATOMEX_SUPPORT_INSERTION_DSP
#define CRIATOMEX_SUPPORT_STANDARD_VOICE_POOL
#define CRIATOMEX_SUPPORT_WAVE_VOICE_POOL

using System;
using System.Runtime.InteropServices;

/*==========================================================================
 *      CRI Atom Native Wrapper
 *=========================================================================*/
/**
 * \addtogroup CRIATOM_NATIVE_WRAPPER
 * @{
 */


/**
 * <summary>ボイスプールの制御を行うためのクラスです。</summary>
 * \par 説明:
 * ボイスプールの制御を行うためのクラスです。<br/>
 */
public abstract class CriAtomExVoicePool : CriDisposable
{
	/* @cond DOXYGEN_IGNORE */
	public const int StandardMemoryAsrVoicePoolId		= 0;	/**< ASRによる標準メモリ再生ボイスプールID */
	public const int StandardStreamingAsrVoicePoolId	= 1;	/**< ASRによる標準ストリーミング再生ボイスプールをID */
	public const int StandardMemoryNsrVoicePoolId		= 2;	/**< NSRによる標準メモリ再生ボイスプールID */
	public const int StandardStreamingNsrVoicePoolId	= 3;	/**< NSRによる標準ストリーミング再生ボイスプールID */
	/* @endcond */
	
	/**
	 * <summary>プラグイン内部で生成するボイスプールへアクセスするためのID</summary>
	 * \sa CriAtomExVoicePool.GetNumUsedVoices
	 */
	public enum VoicePoolId
	{
		/* 機種共通のボイスプールID */
#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_ANDROID || UNITY_IOS || UNITY_TVOS
		StandardMemory			= StandardMemoryAsrVoicePoolId,		/**< 機種標準のメモリ再生ボイスプールID */
		StandardStreaming		= StandardStreamingAsrVoicePoolId,	/**< 機種標準のストリーミング再生ボイスプールID */
#else
		#error unsupported platform
#endif
		HcaMxMemory				= 4,								/**< HCA-MXメモリ再生ボイスプールID */
		HcaMxStreaming			= 5,								/**< HCA-MXストリーミング再生ボイスプールID */

		/* 機種固有のボイスプールID */
#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_IOS || UNITY_TVOS
#elif UNITY_ANDROID
		LowLatencyMemory		= StandardMemoryNsrVoicePoolId,		/**< [Android] 低遅延メモリ再生ボイスプールID */
		LowLatencyStreaming		= StandardStreamingNsrVoicePoolId,	/**< [Android] 低遅延ストリーミング再生ボイスプールID */
#else
#error unsupported platform
#endif
    }

	/**
	 * <summary>ピッチシフタDSP動作モード</summary>
	 * \par 説明:
	 * ピッチシフトの処理方法（アルゴリズム）を指定します。
	 * \sa CriAtomExVoicePool.AttachDspPitchShifter, CriAtomExPlayer.SetDspParameter
	 */
	public enum PitchShifterMode : int {
		Music		= 0,
		Vocal		= 1,
		SoundEffect	= 2,
		Speech		= 3
	};

    /**
	 * <summary>ボイスプールのボイス使用状況を表すための構造体</summary>
	 * \sa CriAtomExVoicePool.GetNumUsedVoices
	 */
    public struct UsedVoicesInfo
	{
		public int numUsedVoices;	/**< 使用中のボイス数 */
		public int numPoolVoices;	/**< ボイスプールのボイス数 */
	}
	
	/**
	 * <summary>ボイスプールのボイス使用状況取得</summary>
	 * <param name="voicePoolId">ボイスプールのID</param>
	 * <returns>ボイス使用状況</returns>
	 * \par 説明:
	 * 指定されたボイスプールのボイス使用状況を取得します。
	 * \attention
	 * 本関数はデバッグ目的でのみ使用してください。
	 * \sa CriAtomExVoicePool::VoicePoolId, CriAtomExVoicePool::UsedVoicesInfo
	 */
	static public UsedVoicesInfo GetNumUsedVoices(VoicePoolId voicePoolId)
	{
		UsedVoicesInfo info;
		criAtomUnity_GetNumUsedVoices((int)voicePoolId, out info.numUsedVoices, out info.numPoolVoices);
		return info;
	}


	public IntPtr nativeHandle {get {return this._handle;} }
	public uint identifier {get {return this._identifier;} }
	public int numVoices {get {return this._numVoices; } }
	public int maxChannels {get {return this._maxChannels; } }
	public int maxSamplingRate {get {return this._maxSamplingRate; } }

	/**
	 * <summary>ボイスプールの破棄</summary>
	 * ボイスプールオブジェクトの破棄を行います。<br>
	 * 作成したオブジェクトを本APIで破棄しない場合、リソースリークが発生しますので必ず破棄してください。
	 */
	public override void Dispose()
	{
		CriDisposableObjectManager.Unregister(this);
		if (this._handle != IntPtr.Zero) {
			CriAtomExVoicePool.criAtomExVoicePool_Free(this._handle);
		}
		GC.SuppressFinalize(this);
	}

	/**
	 * <summary>ボイスプールのボイス使用状況取得</summary>
	 * <returns>ボイス使用状況</returns>
	 * \par 説明:
	 * ボイス使用状況を取得します。
	 * \attention
	 * 本関数はデバッグ目的でのみ使用してください。
	 * \sa CriAtomExVoicePool::UsedVoicesInfo
	 */
	public UsedVoicesInfo GetNumUsedVoices()
	{
		UsedVoicesInfo info;
		criAtomExVoicePool_GetNumUsedVoices(this._handle, out info.numUsedVoices, out info.numPoolVoices);
		return info;
	}

#if CRIATOMEX_SUPPORT_INSERTION_DSP
	/**
	 * <summary>タイムストレッチDSPのアタッチ</summary>
	 * \par 説明:
	 * ボイスプールにタイムストレッチDSPを追加します。
	 * \attention
	 * 本関数は完了復帰型の関数です。<br>
	 * 本関数を実行すると、しばらくの間Atomライブラリのサーバ処理がブロックされます。<br>
	 * 音声再生中に本関数を実行すると、音途切れ等の不具合が発生する可能性があるため、 本関数の呼び出しは
	 * シーンの切り替わり等、負荷変動を許容できるタイミングで行ってください。
	 * \sa CriAtomExVoicePool::DetachInsertionDsp
	 */
	public void AttachDspTimeStretch()
	{
		ExTimeStretchConfig config;
		config.numDsp = this._numVoices;
		config.maxChannels = this._maxChannels;
		config.maxSamplingRate = this._maxSamplingRate;
		config.config.reserved = 0;
		criAtomExVoicePool_AttachDspTimeStretch(this._handle, ref config, IntPtr.Zero, 0);
	}

	/**
	 * <summary>ピッチシフタDSPのアタッチ</summary>
	 * <param name="mode">ピッチシフトモード</param>
	 * <param name="windosSize">ウィンドウサイズ</param>
	 * <param name="overlapTimes">オーバーラップ回数</param>
	 * \par 説明:
	 * ボイスプールにピッチシフタDSPを追加します。
	 * \attention
	 * 本関数は完了復帰型の関数です。<br>
	 * 本関数を実行すると、しばらくの間Atomライブラリのサーバ処理がブロックされます。<br>
	 * 音声再生中に本関数を実行すると、音途切れ等の不具合が発生する可能性があるため、 本関数の呼び出しは
	 * シーンの切り替わり等、負荷変動を許容できるタイミングで行ってください。 
	 * \sa CriAtomExVoicePool::DetachInsertionDsp
	 */
	public void AttachDspPitchShifter(PitchShifterMode mode = PitchShifterMode.Music, int windosSize = 1024, int overlapTimes = 4)
	{
		ExPitchShifterConfig config;
		config.numDsp = this._numVoices;
		config.maxChannels = this._maxChannels;
		config.maxSamplingRate = this._maxSamplingRate;
		config.config.mode = (int)mode;
		config.config.windowSize = windosSize;
		config.config.overlapTimes = overlapTimes;
		criAtomExVoicePool_AttachDspPitchShifter(this._handle, ref config, IntPtr.Zero, 0);
	}

	/**
	 * <summary>DSPのデタッチ</summary>
	 * \par 説明:
	 * ボイスプールに追加したDSPを取り外します。
	 * \attention
	 * 本関数は完了復帰型の関数です。<br>
	 * 本関数を実行すると、しばらくの間Atomライブラリのサーバ処理がブロックされます。<br>
	 * 音声再生中に本関数を実行すると、音途切れ等の不具合が発生する可能性があるため、 本関数の呼び出しは
	 * シーンの切り替わり等、負荷変動を許容できるタイミングで行ってください。
	 * \sa CriAtomExVoicePool::AttachDspPitchShifter, CriAtomExVoicePool::AttachDspTimeStretch
	 */
	public void DetachDsp()
	{
		criAtomExVoicePool_DetachDsp(this._handle);
	}
#endif

	#region Internal Members

	~CriAtomExVoicePool()
	{
		Dispose();
	}

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
	protected struct PlayerConfig
	{
		public int maxChannels;
		public int maxSamplingRate;
		public bool streamingFlag;
		public int soundRendererType;
		public int decodeLatency;
	}

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
	protected struct VoicePoolConfig
	{
		public uint identifier;
		public int numVoices;
		public PlayerConfig playerConfig;
	}

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
	private struct PitchShifterConfig {
		public int mode;
		public int windowSize;
		public int overlapTimes;
	}

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
	private struct ExPitchShifterConfig {
		public int numDsp;
		public int maxChannels;
		public int maxSamplingRate;
		public PitchShifterConfig config;
	}

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
	private struct TimeStretchConfig {
		public int reserved;
	}

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
	private struct ExTimeStretchConfig {
		public int numDsp;
		public int maxChannels;
		public int maxSamplingRate;
		public TimeStretchConfig config;
	}

	protected IntPtr _handle = IntPtr.Zero;
	protected uint _identifier = 0;
	protected int _numVoices = 0;
	protected int _maxChannels = 0;
	protected int _maxSamplingRate = 0;

	#endregion

	#region DLL Import
	#if !CRIWARE_ENABLE_HEADLESS_MODE
	[DllImport(CriWare.pluginName, CallingConvention = CriWare.pluginCallingConvention)]
	private static extern void criAtomUnity_GetNumUsedVoices(int voice_pool_id, out int num_used_voices, out int num_pool_voices);

	[DllImport(CriWare.pluginName, CallingConvention = CriWare.pluginCallingConvention)]
	private static extern void criAtomExVoicePool_GetNumUsedVoices(IntPtr pool, out int num_used_voices, out int num_pool_voices);

	[DllImport(CriWare.pluginName, CallingConvention = CriWare.pluginCallingConvention)]
	public static extern void criAtomExVoicePool_Free(IntPtr pool);

	#if CRIATOMEX_SUPPORT_INSERTION_DSP
	[DllImport(CriWare.pluginName, CallingConvention = CriWare.pluginCallingConvention)]
	private static extern void criAtomExVoicePool_AttachDspTimeStretch(IntPtr pool, ref ExTimeStretchConfig config, IntPtr work, int work_size) ;

	[DllImport(CriWare.pluginName, CallingConvention = CriWare.pluginCallingConvention)]
	private static extern void criAtomExVoicePool_AttachDspPitchShifter(IntPtr pool, ref ExPitchShifterConfig config, IntPtr work, int work_size) ;

	[DllImport(CriWare.pluginName, CallingConvention = CriWare.pluginCallingConvention)]
	private static extern void criAtomExVoicePool_DetachDsp(IntPtr pool);
	#endif
	#else
	private static void criAtomUnity_GetNumUsedVoices(int voice_pool_id, out int num_used_voices, out int num_pool_voices)
		{ num_used_voices = num_pool_voices = 0; }
	private static void criAtomExVoicePool_GetNumUsedVoices(IntPtr pool, out int num_used_voices, out int num_pool_voices)
		{ num_used_voices = num_pool_voices = 0; }
	public static void criAtomExVoicePool_Free(IntPtr pool) { }	
	#if CRIATOMEX_SUPPORT_INSERTION_DSP
	private static void criAtomExVoicePool_AttachDspTimeStretch(IntPtr pool, ref ExTimeStretchConfig config, IntPtr work, int work_size) { }
	private static void criAtomExVoicePool_AttachDspPitchShifter(IntPtr pool, ref ExPitchShifterConfig config, IntPtr work, int work_size) { }
	private static void criAtomExVoicePool_DetachDsp(IntPtr pool) { }
	#endif
	#endif
	#endregion
}

#if CRIATOMEX_SUPPORT_STANDARD_VOICE_POOL

/**
 * <summary>標準ボイスプール</summary>
 */
public class CriAtomExStandardVoicePool: CriAtomExVoicePool
{
	/**
	 * <summary>追加標準ボイスプールの作成</summary>
	 * <param name="numVoices">ボイス数</param>
	 * <param name="maxChannels">最大チャンネル数</param>
	 * <param name="maxSamplingRate">最大サンプリングレート</param>
	 * <param name="streamingFlag">ストリーミング再生フラグ</param>
	 * <param name="identifier">ボイスプール識別子</param>
	 * <returns>標準ボイスプール</returns>
	 * 標準ボイスプールを追加で作成します。<br>
	 * 6 チャンネル以上の音声を再生する場合、本 API でボイスプールを作成してください。<br>
	 * 再生終了後は、必ず、Dispose 関数でオブジェクトを破棄してください。<br>
	 * 特定の CriAtomExPlayer に対して、作成したボイスプールからボイスを取得するように明示的に設定したい場合は、
	 * identifier としてデフォルトの 0 以外の値を指定して作成し、 CriAtomExPlayer::SetVoicePoolIdentifier 関数を
	 * 呼び出してください。
	 * \sa CriAtomExPlayer::SetVoicePoolIdentifier
	 */
	public CriAtomExStandardVoicePool(int numVoices, int maxChannels, int maxSamplingRate, bool streamingFlag, uint identifier = 0)
	{
		this._identifier = identifier;
		this._numVoices = numVoices;
		this._maxChannels = maxChannels;
		this._maxSamplingRate = maxSamplingRate;

		VoicePoolConfig config = new VoicePoolConfig();
		config.identifier = identifier;
		config.numVoices = numVoices;
		config.playerConfig.maxChannels = maxChannels;
		config.playerConfig.maxSamplingRate = maxSamplingRate;
		config.playerConfig.streamingFlag = streamingFlag;
		config.playerConfig.soundRendererType = (int)CriAtomEx.SoundRendererType.Asr;
		config.playerConfig.decodeLatency = 0;
		this._handle = criAtomExVoicePool_AllocateStandardVoicePool(ref config, IntPtr.Zero, 0);
		if (this._handle == IntPtr.Zero) {
			throw new Exception("CriAtomExStandardVoicePool() failed.");
		}
		
		CriDisposableObjectManager.Register(this, CriDisposableObjectManager.ModuleType.Atom);
	}

	#region DLL Import
	#if !CRIWARE_ENABLE_HEADLESS_MODE
	[DllImport(CriWare.pluginName)]
	private static extern IntPtr criAtomExVoicePool_AllocateStandardVoicePool(ref VoicePoolConfig config, IntPtr work, int work_size);
	#else
	private static IntPtr criAtomExVoicePool_AllocateStandardVoicePool(ref VoicePoolConfig config, IntPtr work, int work_size) { return new IntPtr(1); }
	#endif
	#endregion
}

#endif

#if CRIATOMEX_SUPPORT_WAVE_VOICE_POOL

/**
 * <summary>Wave ボイスプール</summary>
 */
public class CriAtomExWaveVoicePool: CriAtomExVoicePool
{
	/**
	 * <summary>Wave ボイスプールの作成</summary>
	 * <param name="numVoices">ボイス数</param>
	 * <param name="maxChannels">最大チャンネル数</param>
	 * <param name="maxSamplingRate">最大サンプリングレート</param>
	 * <param name="streamingFlag">ストリーミング再生フラグ</param>
	 * <param name="identifier">ボイスプール識別子</param>
	 * <returns>Wave ボイスプール</returns>
	 * 本関数を実行することで、Wave 再生が可能なボイスがプールされます。<br>
	 * AtomEx プレーヤで Wave データ（もしくは Wave データを含むキュー）の再生を行うと、
	 * AtomEx プレーヤは作成された Wave ボイスプールからボイスを取得し、再生を行います。<br>
	 * 再生終了後は、必ず、Dispose 関数でオブジェクトを破棄してください。<br>
	 * 特定の CriAtomExPlayer に対して、作成したボイスプールからボイスを取得するように明示的に設定したい場合は、
	 * identifier としてデフォルトの 0 以外の値を指定して作成し、 CriAtomExPlayer::SetVoicePoolIdentifier 関数を
	 * 呼び出してください。
	 * \sa CriAtomExPlayer::SetVoicePoolIdentifier
	 */
	public CriAtomExWaveVoicePool(int numVoices, int maxChannels, int maxSamplingRate, bool streamingFlag, uint identifier = 0)
	{
		this._identifier = identifier;
		this._identifier = identifier;
		this._numVoices = numVoices;
		this._maxChannels = maxChannels;
		this._maxSamplingRate = maxSamplingRate;

		VoicePoolConfig config = new VoicePoolConfig();
		config.identifier = identifier;
		config.numVoices = numVoices;
		config.playerConfig.maxChannels = maxChannels;
		config.playerConfig.maxSamplingRate = maxSamplingRate;
		config.playerConfig.streamingFlag = streamingFlag;
		config.playerConfig.soundRendererType = (int)CriAtomEx.SoundRendererType.Asr;
		config.playerConfig.decodeLatency = 0;
		this._handle = criAtomExVoicePool_AllocateWaveVoicePool(ref config, IntPtr.Zero, 0);
		if (this._handle == IntPtr.Zero) {
			throw new Exception("CriAtomExWaveVoicePool() failed.");
		}

		CriDisposableObjectManager.Register(this, CriDisposableObjectManager.ModuleType.Atom);
	}

	#region DLL Import
	#if !CRIWARE_ENABLE_HEADLESS_MODE
	[DllImport(CriWare.pluginName)]
	private static extern IntPtr criAtomExVoicePool_AllocateWaveVoicePool(ref VoicePoolConfig config, IntPtr work, int work_size);
	#else
	private static IntPtr criAtomExVoicePool_AllocateWaveVoicePool(ref VoicePoolConfig config, IntPtr work, int work_size) { return new IntPtr(1); }
	#endif
	#endregion
}

#endif

/**
 * @}
 */

/* --- end of file --- */

