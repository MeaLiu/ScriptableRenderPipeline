using System;

namespace UnityEngine.Experimental.Rendering.HDPipeline
{
    /// <summary>Settings that defines the rendering of a probe.</summary>
    [Serializable]
    public struct ProbeSettings
    {
        /// <summary>The type of the probe.</summary>
        public enum ProbeType
        {
            /// <summary>
            /// Standard reflection probe.
            ///
            /// A reflection probe captures a cubemap around a capture position.
            /// </summary>
            ReflectionProbe,
            /// <summary>
            /// Planar reflection probe.
            ///
            /// A planar reflection probe captures a single camera render.
            /// The capture position is the mirrored viewer's position against a mirror plane.
            /// This plane is defined by the probe's transform:
            ///  * center = center of the probe
            ///  * normal = forward of the probe
            ///
            /// The viewer's transform must be provided with <see cref="ProbeCapturePositionSettings.referencePosition"/>
            /// and <see cref="ProbeCapturePositionSettings.referenceRotation"/> when calling <see cref="HDRenderUtilities.Render(ProbeSettings, ProbeCapturePositionSettings, Texture)"/>.
            /// </summary>
            PlanarProbe
        }

        /// <summary>The rendering mode of the probe.</summary>
        public enum Mode
        {
            /// <summary>Capture data is baked in editor and loaded as assets.</summary>
            Baked,
            /// <summary>Capture data provided as an assets.</summary>
            Custom,
            /// <summary>Capture data is computed during runtime.</summary>
            Realtime
        }

        /// <summary>Lighting parameters for the probe.</summary>
        [Serializable]
        public struct Lighting
        {
            /// <summary>Default value.</summary>
            public static readonly Lighting @default = new Lighting
            {
                multiplier = 1.0f,
                weight = 1.0f
            };

            /// <summary>A multiplier applied to the radiance of the probe.</summary>
            public float multiplier;
            /// <summary>A weight applied to the influence of the probe.</summary>
            public float weight;
        }

        /// <summary>Settings of this probe in the current proxy.</summary>
        [Serializable]
        public struct ProxySettings
        {
            /// <summary>Default value.</summary>
            public static readonly ProxySettings @default = new ProxySettings
            {
                capturePositionProxySpace = Vector3.zero,
                captureRotationProxySpace = Quaternion.identity,
                useInfluenceVolumeAsProxyVolume = false
            };

            /// <summary>
            /// Whether to use the influence volume as proxy volume
            /// when <c><see cref="linkedProxy"/> == null</c>.
            /// </summary>
            public bool useInfluenceVolumeAsProxyVolume;
            /// <summary>Position of the capture in proxy space. (Reflection Probe only)</summary>
            public Vector3 capturePositionProxySpace;
            /// <summary>Rotation of the capture in proxy space. (Reflection Probe only)</summary>
            public Quaternion captureRotationProxySpace;
            /// <summary>Position of the mirror in proxy space. (Planar Probe only)</summary>
            public Vector3 mirrorPositionProxySpace;
            /// <summary>Rotation of the mirror in proxy space. (Planar Probe only)</summary>
            public Quaternion mirrorRotationProxySpace;
        }

        /// <summary>Default value.</summary>
        public static ProbeSettings @default = new ProbeSettings
        {
            type = ProbeType.ReflectionProbe,
            mode = Mode.Baked,
            camera = CameraSettings.@default,
            influence = null,
            lighting = Lighting.@default,
            linkedProxy = null,
            proxySettings = ProxySettings.@default
        };

        /// <summary>The type of the probe.</summary>
        public ProbeType type;
        /// <summary>The mode of the probe.</summary>
        public Mode mode;
        /// <summary>The lighting of the probe.</summary>
        public Lighting lighting;
        /// <summary>The influence volume of the probe.</summary>
        public InfluenceVolume influence;
        /// <summary>Set this variable to explicitly set the proxy volume to use.</summary>
        public ProxyVolume linkedProxy;
        /// <summary>The proxy settings of the probe for the current volume.</summary>
        public ProxySettings proxySettings;
        /// <summary>Camera settings to use when capturing data.</summary>
        public CameraSettings camera;

        public Hash128 ComputeHash()
        {
            var h = new Hash128();
            var h2 = new Hash128();
            HashUtilities.ComputeHash128(ref type, ref h);
            HashUtilities.ComputeHash128(ref mode, ref h2);
            HashUtilities.AppendHash(ref h2, ref h);
            HashUtilities.ComputeHash128(ref lighting, ref h2);
            HashUtilities.AppendHash(ref h2, ref h);
            HashUtilities.ComputeHash128(ref proxySettings, ref h2);
            HashUtilities.AppendHash(ref h2, ref h);
            HashUtilities.ComputeHash128(ref camera, ref h2);
            HashUtilities.AppendHash(ref h2, ref h);
            if (influence != null)
            {
                h2 = influence.ComputeHash();
                HashUtilities.AppendHash(ref h2, ref h);
            }
            if (linkedProxy != null)
            {
                h2 = linkedProxy.ComputeHash();
                HashUtilities.AppendHash(ref h2, ref h);
            }
            return h;
        }
    }
}
