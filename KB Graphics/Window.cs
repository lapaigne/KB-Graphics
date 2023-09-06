using OpenTK.Graphics.OpenGL4;
using OpenTK.Platform.Windows;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace KB_Graphics
{
    public class Window : GameWindow
    {
        private readonly float[] _vertices =
        {
            0.5f,  0.5f, 0.0f, 1f, 1f, // top right
             0.5f, -0.5f, 0.0f, 1f, 0f, // bottom right
            -0.5f, -0.5f, 0.0f, 0f, 0f, // bottom left
            -0.5f,  0.5f, 0.0f, 0f, 1f  // top left
        };

        //texture scheme
        /* 
         * (0,1)...  ...(1,1)
         * ...          ...
         * ...          ...
         * (0,0)...  ...(1,0)
         */

        uint[] _indices =
        {
            0, 1, 3,
            1, 2, 3
        };

        int _elementBufferObject;
        private int _vertexBufferObject;
        private int _vertexArrayObject;
        private Shader _shader;
        private Texture _texture;
        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings) 
        {
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            GL.ClearColor(0.1f, 0.4f, 0.4f, 1.0f);
            
            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);
            
            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

            _elementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);
            
            _shader = new Shader("Shaders/shader.vert", "Shaders/shader.frag");
            _shader.Use();

            var vertexLocation = _shader.GettAttributeLocation("aPosition");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

            var textureLocation = _shader.GettAttributeLocation("aTexCoord");
            GL.EnableVertexAttribArray(textureLocation);
            GL.VertexAttribPointer(textureLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));

            _texture = Texture.LoadFromFile("Resources/Textures/concrete.jpg");
            _texture.Use(TextureUnit.Texture0);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            
            GL.Clear(ClearBufferMask.ColorBufferBit);

            GL.BindVertexArray(_vertexArrayObject);

            _texture.Use(TextureUnit.Texture0);
            _shader.Use();

            GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);
            
            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            var input = KeyboardState;

            if (input.IsKeyDown(Keys.Escape))
            {
                Close();
            }
        }

        protected override void OnResize(ResizeEventArgs args)
        {
            base.OnResize(args);
            GL.Viewport(0, 0, Size.X, Size.Y);
        }

        protected override void OnUnload()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.UseProgram(0);

            GL.DeleteBuffer(_vertexBufferObject);
            GL.DeleteVertexArray(_vertexArrayObject);

            GL.DeleteProgram(_shader.Handle);

            base.OnUnload();
        }
    }
}