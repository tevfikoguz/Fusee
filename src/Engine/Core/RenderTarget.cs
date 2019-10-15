using Fusee.Base.Common;
using Fusee.Engine.Common;
using System;

namespace Fusee.Engine.Core
{    
    
    /// <summary>
    /// Render to a target if you want to render to a texture and use them in an other pass.
    /// Use the "Create__Tex"-Methods to generate the textures you need. The order of the textures in the RenderTextures array is given by the <see cref="RenderTargetTextures"/> enum.
    /// </summary>
    public class RenderTarget : IRenderTarget, IDisposable
    {
        /// <summary>
        /// Event that deletes unmanaged buffer objects.
        /// </summary>
        public event EventHandler<EventArgs> DeleteBuffers;

        /// Flag: Has Dispose already been called?
        public bool Disposed { get; private set; } = false;

        ///Order of textures in RenderTextures array is given by the corresponding enum.
        public IWritableTexture[] RenderTextures { get; private set; }

        public IWritableCubeMap CubeMap { get; private set; }

        /// <summary>
        /// Handle of the corresponding G-Buffer. Used to dispose the object if it isn't needed anymore.
        /// </summary>
        public IBufferHandle GBufferHandle { get; set; }

        /// <summary>
        /// Handle of the corresponding Depth Buffer (as renderbuffer). Used to dispose the object if it isn't needed anymore.
        /// </summary>
        public IBufferHandle DepthBufferHandle { get; set; }

        /// <summary>
        /// Sets the resolution of the render textures.
        /// </summary>
        public TexRes TextureResolution { get; private set; }

        /// <summary>
        /// If the RenderTarget only contains a depth texture or a depth cube map, there will be no draw buffers.
        /// </summary>
        public bool IsDepthOnly { get; set ; }        

        /// <summary>
        /// Creates a new instance of type "RenderTarget".
        /// </summary>
        /// <param name="texRes">Resolution of the created Textures.</param>
        public RenderTarget(TexRes texRes)
        {           
            RenderTextures = new WritableTexture[Enum.GetNames(typeof(RenderTargetTextures)).Length];
            TextureResolution = texRes;
            IsDepthOnly = false;
        }

        public void CreateCubeMap(RenderTargetTextures texType, ColorFormat colorFormat)
        {
            CubeMap = new WritableCubeMap(texType, new ImagePixelFormat(colorFormat), (int)TextureResolution, (int) TextureResolution, false, TextureFilterMode.NEAREST, TextureWrapMode.CLAMP_TO_EDGE);
            
        }

        /// <summary>
        /// Sets a RenderTexture from another RenderTarget at the correct position in the RenderTexure array.
        /// </summary>
        /// <param name="src">The source RenderTarget.</param>
        /// <param name="tex">The type of the texture.</param>
        public void SetTextureFromRenderTarget(RenderTarget src, RenderTargetTextures tex)
        {
            var srcTex = src.RenderTextures[(int)tex];
            RenderTextures[(int)tex] = srcTex ?? throw new ArgumentException("Texture from source target is null!");
        }         
        

        /// <summary>
        /// Generates a position texture and sets it at the correct position in the RenderTextures Array.
        /// </summary>
        public void SetPositionTex()
        {          
            RenderTextures[(int)RenderTargetTextures.G_POSITION] = WritableTexture.CreatePosTex((int)TextureResolution, (int)TextureResolution);
        }

        /// <summary>
        /// Generates a albedo and specular (alpha channel) texture and sets it at the correct position in the RenderTextures Array.
        /// </summary>       
        public void SetAlbedoSpecularTex()
        {
            RenderTextures[(int)RenderTargetTextures.G_ALBEDO_SPECULAR] = WritableTexture.CreateAlbedoSpecularTex((int)TextureResolution, (int)TextureResolution);
        }

        /// <summary>
        /// Generates a normal texture and sets it at the correct position in the RenderTextures Array.
        /// </summary>
        public void SetNormalTex()
        {
            RenderTextures[(int)RenderTargetTextures.G_NORMAL] = WritableTexture.CreateNormalTex((int)TextureResolution, (int)TextureResolution);
        }

        /// <summary>
        /// Generates a depth texture and sets it at the correct position in the RenderTextures Array.
        /// </summary>
        public void SetDepthTex()
        {
            RenderTextures[(int)RenderTargetTextures.G_DEPTH] = WritableTexture.CreateDepthTex((int)TextureResolution, (int)TextureResolution);
        }

        /// <summary>
        /// Generates a ssao texture and sets it at the correct position in the RenderTextures Array.
        /// </summary>        
        public void SetSSAOTex()
        { 
            RenderTextures[(int)RenderTargetTextures.G_SSAO] = WritableTexture.CreateSSAOTex((int)TextureResolution, (int)TextureResolution);
        }        

        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (Disposed)
                return;

            if (disposing)
            {
                //Dispose buffers here
                DeleteBuffers?.Invoke(this, new EventArgs());
            }

            Disposed = true;
        }

        ~RenderTarget()
        {           
            Dispose(false);
        }

    }
}
