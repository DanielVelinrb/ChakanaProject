﻿/* Color Correction Pro
 * Version: 0.9.8
 * Copyright © 2017-2020 by Uniarts
 */

using UnityEngine;

namespace Uniarts.ColorCorrection {

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
[AddComponentMenu("Image Effects/Color Adjustments/Color Correction Pro")]
	public class ColorCorrectionPro : MonoBehaviour {
	
	[Range(-180.0f, 180.0f)] public float hue = 0.0f;
	[Range(-100.0f, 100.0f)] public float saturation = 0.0f;
	[Range(-100.0f, 100.0f)] public float lightness = 0.0f;

	[Range(-100.0f, 100.0f)] public float brightness = 0.0f;
	[Range(-100.0f, 100.0f)] public float contrast = 0.0f;
	[Range(-100.0f, 100.0f)] public float gamma = 0.0f;
	[Range(-100.0f, 100.0f)] public float sharpness = 0.0f;
    [Range(-100.0f, 100.0f)] public float temperature = 0.0f;
    [Range(0, 100.0f)] public float threshold = 0.0f;

	[Range(-100.0f, 100.0f)] public float redIntensity = 0.0f;
	[Range(-100.0f, 100.0f)] public float greenIntensity = 0.0f;
	[Range(-100.0f, 100.0f)] public float blueIntensity = 0.0f;

	public AnimationCurve redCurve = AnimationCurve.Linear(0f, 0f, 1.0f, 1.0f);
	public AnimationCurve greenCurve = AnimationCurve.Linear(0f, 0f, 1.0f, 1.0f);
	public AnimationCurve blueCurve = AnimationCurve.Linear(0f, 0f, 1.0f, 1.0f);

    // Texture for Curves
	private Texture2D _curvesRGBTex; 

	public bool basic = true;
	public bool colorBalance = false;
	public bool colorCurves = false;
	public bool levels = false;
	public float inputLevelBlack = 0.0f;
	public float inputLevelWhite = 255.0f;
	public float outputLevelBlack = 0.0f;
	public float outputLevelWhite = 255.0f;

    public enum PhotoFilterMode { Presets = 0, Select = 1 }
    public enum LutFilterMode { Presets = 0, Select = 1 }
    public enum RampFilterMode { Presets = 0, Select = 1 }
	
    public enum PhotoPresets {
            WarmingFilter85, WarmingFilterLBA, WarmingFilter81,
            CoolingFilter80, CoolingFilterLBB, CoolingFilter82,

            Red, Orange, Yellow,
            Green, Cyan, Blue,
            Violet, Magenta, White,

            Sepia, DeepRed, DeepBlue,
            DeepEmerald, DeepYellow, Underwater
    }

    public enum LutPresets {
            Autumn, Winter, Spring, Summer,
            Arctic, Ice, Tropical, GreenField,
            SunnyDay, Sunrise, Sunset, MoonNight, 
            Cyber, Modern, Dream, Nostalgic, Serenity, Lemon, Swamp, 
            PostWar, RedZone, SciFi, WildWest,
            Pencil, Grayscale, BlackAndWhite,
            OldPhoto, Retro, Neutral
    }

    public enum RampPresets {           
            Red, RedThreshold, RedThresholdStylized,
            Green, GreenThreshold, GreenThresholdStylized,
            Blue, BlueThreshold, BlueThresholdStylized, BlueOrangeThreshold, LilacPinkThreshold, RedZone, SciFi,
            Gold, BrownThresholdStylized, WildWest,
            Grayscale, BlackAndWhite, BlackAndWhiteThreshold, BlackGrayWhiteThreshold
    }
			
    public PhotoFilterMode photoFilterMode = PhotoFilterMode.Presets;
    public LutFilterMode lutFilterMode = LutFilterMode.Presets;
    public RampFilterMode rampFilterMode = RampFilterMode.Presets;

    public PhotoPresets photoPreset = PhotoPresets.White;
    public LutPresets lutPreset = LutPresets.Neutral;
	
	protected LutPresets _previousLutPreset = LutPresets.Neutral;

    public RampPresets rampPreset = RampPresets.BlackAndWhite;

    public bool photoFilter = false;
    public Color colorForPhotoFilter = Color.white;
    [Range(0.0f, 100.0f)] public float photoFilterIntensity = 0.0f;
	
	public bool lutFilter = false;
	public Texture2D lutTexture = null;
	private Texture2D _previousLutTexture = null;

	// Converted 3D Lut
    public Texture3D lut3DTexture = null;
	[Range(0.0f, 100.0f)] public float lutFilterIntensity = 0.0f;

	public bool rampFilter = false;
	public Texture rampTexture = null;
    [Range(0.0f, 100.0f)] public float rampFilterIntensity = 0.0f;

	public bool compareMode = false;

    public FilterMode filterMode = FilterMode.Bilinear;

  	public Shader correctionShader;
    private Material _correctionMaterial;
	private Material _separableBlurMaterial;

    private Material material {
        get {

            if ( _correctionMaterial == null ) {
                 _correctionMaterial = new Material(correctionShader);
                 _correctionMaterial.hideFlags = HideFlags.HideAndDontSave;
            }

            return _correctionMaterial;
        }
    }

    void OnEnable() {
        if ( GetComponent<Camera>() == null ) {
            Debug.LogError("This script must be attached to a Camera");
        }
    }

    void OnDisable() {
        if ( _correctionMaterial ) { 
			DestroyImmediate( _correctionMaterial ); 
		}
		_correctionMaterial = null;
    }

    void OnDestroy() {
        if ( lut3DTexture != null ) { DestroyImmediate( lut3DTexture ); }
        lut3DTexture = null;
    }

    void Start() {

        if ( !correctionShader && !correctionShader.isSupported ) {
            enabled = false;
        } else {
			enabled = true;
            return;
		}

    }

    void Update () {
			
		hue = Mathf.Clamp(hue, -180.0f, 180.0f);
		saturation = Mathf.Clamp(saturation, -100.0f, 100.0f);
		lightness = Mathf.Clamp(lightness, -100.0f, 100.0f);
		brightness = Mathf.Clamp(brightness, -100.0f, 100.0f);
		contrast = Mathf.Clamp(contrast, -100.0f, 100.0f);
		gamma = Mathf.Clamp(gamma, -100.0f, 100.0f);
		sharpness = Mathf.Clamp(sharpness, -100.0f, 100.0f);
        temperature = Mathf.Clamp(temperature, -100.0f, 100.0f);
		threshold = Mathf.Clamp(threshold, 0.0f, 100.0f);

		redIntensity = Mathf.Clamp(redIntensity, -100.0f, 100.0f);
		greenIntensity = Mathf.Clamp(greenIntensity, -100.0f, 100.0f);
		blueIntensity = Mathf.Clamp(blueIntensity, -100.0f, 100.0f);

        inputLevelBlack = Mathf.Clamp(inputLevelBlack, 0.0f, 255.0f);
		inputLevelWhite = Mathf.Clamp(inputLevelWhite, 0.0f, 255.0f);
		outputLevelBlack = Mathf.Clamp(outputLevelBlack, 0.0f, 255.0f);
		outputLevelWhite = Mathf.Clamp(outputLevelWhite, 0.0f, 255.0f);
		
		photoFilterIntensity = Mathf.Clamp(photoFilterIntensity, 0.0f, 100.0f);
		lutFilterIntensity = Mathf.Clamp(lutFilterIntensity, 0.0f, 100.0f);
		rampFilterIntensity = Mathf.Clamp(rampFilterIntensity, 0.0f, 100.0f);

		if (lutFilterMode == LutFilterMode.Select) {															
			if (lutTexture != _previousLutTexture) {
				_previousLutTexture = lutTexture;
				ConvertTo3DLut(lutTexture);
			}
		} else if (lutFilterMode == LutFilterMode.Presets) {
			if (lutPreset != _previousLutPreset) {
				_previousLutPreset = lutPreset;
				lutTexture = Resources.Load<Texture2D>("Presets/LUTs/" + lutPreset.ToString());
				ConvertTo3DLut(lutTexture);
			}
		}

		if (lutTexture == null) { lut3DTexture = null; }
		
		if (rampFilterMode == RampFilterMode.Presets) {
	        rampTexture = Resources.Load<Texture2D>("Presets/Ramps/" + rampPreset.ToString());
	    }

    }

	public void Reset() {
	
        hue = 0.0f;
		saturation = 0.0f;
		lightness = 0.0f;
		brightness = 0.0f;
		contrast = 0.0f;
		gamma = 0.0f;
		sharpness = 0.0f;
        temperature = 0.0f;
        threshold = 0.0f;
		basic = true;

		redIntensity = 0.0f;
		greenIntensity = 0.0f;
		blueIntensity = 0.0f;
		colorBalance = false;

		redCurve = AnimationCurve.Linear(0f, 0f, 1.0f, 1.0f);
		greenCurve = AnimationCurve.Linear(0f, 0f, 1.0f, 1.0f);
		blueCurve = AnimationCurve.Linear(0f, 0f, 1.0f, 1.0f);
		colorCurves = false;

        inputLevelBlack = 0.0f;
        inputLevelWhite = 255.0f;
        outputLevelBlack = 0.0f;
        outputLevelWhite = 255.0f;
		levels = false;

        photoPreset = PhotoPresets.White;
        photoFilterMode = PhotoFilterMode.Presets;
        photoFilterIntensity = 0.0f;
        photoFilter = false;

        lutPreset = LutPresets.Neutral;
        lutFilterMode = LutFilterMode.Presets;        
        lutFilterIntensity = 0.0f;
        lutTexture = null;
        lutFilter = false;

        rampPreset = RampPresets.BlackAndWhite;
        rampFilterMode = RampFilterMode.Presets;
        rampFilterIntensity = 0.0f;
        rampTexture = null;
        rampFilter = false;

        filterMode = FilterMode.Bilinear;
		compareMode = false;

	}

	private void SetBasic() {
		material.SetFloat("_Hue", hue );
		material.SetFloat("_Saturation", saturation / 100.0f + 1.0f);
		material.SetFloat("_Lightness", lightness / 100.0f + 1.0f);
		material.SetFloat("_Brightness", brightness / 100.0f + 1.0f);
		material.SetFloat("_Contrast", contrast / 100.0f + 1.0f);
		material.SetFloat("_Gamma", gamma / 100.0f + 1.0f);
        material.SetFloat("_Temperature", temperature / 200.0f + 1.0f );
        material.SetFloat("_Threshold", threshold / 250.0f);
	}

	private void SetColorBalance() {
		material.SetFloat("_Red", redIntensity/100.0f + 1.0f);     
        material.SetFloat("_Green", greenIntensity/100.0f + 1.0f); 
        material.SetFloat("_Blue", blueIntensity/100.0f + 1.0f);
	}

	private void SetColorCurves() {
		if ( !_curvesRGBTex ) {
				_curvesRGBTex = new Texture2D (256, 4, TextureFormat.ARGB32, false, true);
		}

		_curvesRGBTex.hideFlags = HideFlags.DontSave;
		_curvesRGBTex.wrapMode = TextureWrapMode.Clamp;

		if ( redCurve != null && greenCurve != null && blueCurve != null ) {
			for ( float i = 0.0f; i <= 1.0f; i += 1.0f / 255.0f ) {
				float redChannel = Mathf.Clamp(redCurve.Evaluate(i), 0.0f, 1.0f);
				float greenChannel = Mathf.Clamp(greenCurve.Evaluate(i), 0.0f, 1.0f);
				float blueChannel = Mathf.Clamp(blueCurve.Evaluate(i), 0.0f, 1.0f);

				_curvesRGBTex.SetPixel ((int) Mathf.Floor(i*255.0f), 0, new Color(redChannel, redChannel, redChannel) );
				_curvesRGBTex.SetPixel ((int) Mathf.Floor(i*255.0f), 1, new Color(greenChannel, greenChannel, greenChannel) );
				_curvesRGBTex.SetPixel ((int) Mathf.Floor(i*255.0f), 2, new Color(blueChannel, blueChannel, blueChannel) );

			}
			_curvesRGBTex.Apply();
			material.SetTexture("_CurvesRGBTex", _curvesRGBTex);
		}
	}

	private void SetLevels() {
		material.SetFloat("_InputBlack", inputLevelBlack);
		material.SetFloat("_InputWhite", inputLevelWhite);
		material.SetFloat("_OutputBlack", outputLevelBlack);
		material.SetFloat("_OutputWhite", outputLevelWhite);
	}

	// ------------------------------------ LUT --------------------------------------------------

	public void SetIdentity3DLut() {

		if ( lutTexture ) {
				
			int lut3DSize = lutTexture.height;
			var newColor = new Color[ lut3DSize * lut3DSize * lut3DSize ];
			float oneOverLut3DSize = 1.0f / (1.0f * lut3DSize - 1.0f);

			for(int i = 0; i < lut3DSize; i++) {
				for(int j = 0; j < lut3DSize; j++) {
					for(int k = 0; k < lut3DSize; k++) {
						newColor[i + (j*lut3DSize) + (k*lut3DSize*lut3DSize)] = 
							new Color( (float)i * oneOverLut3DSize, (float)j * oneOverLut3DSize, (float)k * oneOverLut3DSize, 1.0f);
					}
				}
			}

			if (lut3DTexture != null) {
				DestroyImmediate(lut3DTexture);
			}

			lut3DTexture = new Texture3D (lut3DSize, lut3DSize, lut3DSize, TextureFormat.ARGB32, false);
			lut3DTexture.wrapMode = TextureWrapMode.Clamp;
			lut3DTexture.SetPixels(newColor);
			lut3DTexture.Apply ();
			
		}
	}

	public bool Valid3DSize( Texture2D texture2D ) {
		if (!texture2D) { return false; }
		int h = texture2D.height;
		if (h != Mathf.FloorToInt(Mathf.Sqrt(texture2D.width))) { return false; }
		return true;
	}

    public void ConvertTo3DLut( Texture2D aLutTexture ) {

        if ( aLutTexture != null ) {

            int lut3DSize = aLutTexture.height;

            if ( !Valid3DSize(aLutTexture) ) {
                Debug.LogWarning("The given 2D texture " + aLutTexture.name + " cannot be used as a 3D LUT. " +
                    "The height of texture must equal the square root of the width.");
                return;
            }

            var c = aLutTexture.GetPixels();
            var newColor = new Color[c.Length];

            for (int i = 0; i < lut3DSize; i++) {
                for (int j = 0; j < lut3DSize; j++) {
                    for (int k = 0; k < lut3DSize; k++) {

                        newColor[i + (j * lut3DSize * lut3DSize) + (k * lut3DSize)] = c[i + (j * lut3DSize) + (k * lut3DSize * lut3DSize)];

                    }
                }
            }

            if (lut3DTexture != null) {
                DestroyImmediate(lut3DTexture);
            }

            lut3DTexture = new Texture3D(lut3DSize, lut3DSize, lut3DSize, TextureFormat.ARGB32, false);
            lut3DTexture.wrapMode = TextureWrapMode.Clamp;
            lut3DTexture.SetPixels(newColor);

            lut3DTexture.Apply();
                
        }
	}

	//--------------------------------------------------------------------------------------------

	void OnRenderImage(RenderTexture sourceTexture, RenderTexture destinationTexture) {
			
		if(correctionShader != null) {

			sourceTexture.filterMode = this.filterMode;

			material.SetTexture("_MainTex", sourceTexture);
			
			if ( basic ) {
				material.SetInt("_Basic", 1);
				SetBasic();
			} else if ( !basic ) {
				material.SetInt("_Basic", 0);
			}

			if ( colorBalance ) {
				material.SetInt("_ColorBalance", 1);	
				SetColorBalance();
			} else if ( !colorBalance ) {
				material.SetInt("_ColorBalance", 0);		
			}

			if ( colorCurves ) {
				material.SetInt("_ColorCurves", 1);
				SetColorCurves();
			} else if ( !colorCurves ) {
				material.SetInt("_ColorCurves", 0);		
			}

            if ( levels ) {
				material.SetInt("_Levels", 1);	
				SetLevels();
			} else if ( !levels ) {
				material.SetInt("_Levels", 0 );		
			}

			if ( photoFilter ) {
				material.SetInt("_PhotoFilter", 1);	

				material.SetColor("_ColorForPhotoFilter", colorForPhotoFilter); 
				material.SetFloat("_PhotoFilterIntensity", photoFilterIntensity/100.0f);		
			} else if ( !photoFilter ) {
				material.SetInt("_PhotoFilter", 0);		
			}

			if (lut3DTexture == null) {
				SetIdentity3DLut();
			}

			if ( lutFilter && lut3DTexture != null ) {

				int lutSize = lut3DTexture.width;
				material.SetFloat ("_LutScale", (lutSize - 1) / (1.0f * lutSize));
				material.SetFloat ("_LutOffset", 1.0f / (2.0f * lutSize));
				material.SetFloat ("_LutIntensity", lutFilterIntensity/100.0f);
				material.SetTexture ("_LutTex", lut3DTexture);

				material.SetInt("_LutFilter", 1);

			} else if ( !lutFilter ) {
				material.SetInt("_LutFilter", 0);		
			}

			if ( rampFilter && rampTexture != null ) {
				material.SetInt("_RampFilter", 1);
				material.SetTexture("_RampTex", rampTexture);
				material.SetFloat("_RampIntensity", rampFilterIntensity/100.0f);
				
			} else if ( !rampFilter ) {
				material.SetInt("_RampFilter", 0);		
			}

			if ( compareMode ) {
				material.SetInt("_CompareMode", 1);	
			} else if ( !compareMode ) {
				material.SetInt("_CompareMode", 0);		
			}
							
			// ------------------------------- Sharpness --------------------------------

				int rtW = sourceTexture.width;
				int rtH = sourceTexture.height;

				RenderTexture color2 = RenderTexture.GetTemporary (rtW/2, rtH/2, 0);

				Graphics.Blit (sourceTexture, color2);
				RenderTexture color4a = RenderTexture.GetTemporary (rtW/4, rtH/4, 0);
				Graphics.Blit (color2, color4a);
				RenderTexture.ReleaseTemporary (color2);

				material.SetTexture ("_MainTexBlurred", color4a);
				material.SetFloat("_Sharpness", sharpness/100.0f );

			// --------------------------------------------------------------------------

			int pass = QualitySettings.activeColorSpace == ColorSpace.Linear ? 1 : 0;
			Graphics.Blit(sourceTexture, destinationTexture, material, pass);

			// ----------------------------- Sharpness ----------------------------------
				RenderTexture.ReleaseTemporary (color4a);
			// --------------------------------------------------------------------------

		} else {
			Graphics.Blit(sourceTexture, destinationTexture);
		}	

	}


  }

}
