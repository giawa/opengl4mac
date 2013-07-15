using System;

using Tao.FreeGlut;
using OpenGL;

namespace OpenGLTest
{
	public class Program
	{
		private static int width = 1280, height = 720;
		private static VAO cube;

		public static void Main()
		{
			Glut.glutInit ();
			Glut.glutInitDisplayMode(Glut.GLUT_DOUBLE | Glut.GLUT_DEPTH);
			Glut.glutInitWindowSize(width, height);
			Glut.glutCreateWindow("OpenGL");
			Glut.glutIdleFunc (IdleFunc);
			Glut.glutDisplayFunc (DisplayFunc);	// this is required on the Mac implementation of GLUT

			// enable wireframe mode so that we can see the faces of the cube
			Gl.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);

			// create the shader program
			ShaderProgram program = new ShaderProgram(vertexShaderSource, fragmentShaderSource);

			// set the color to blue
			program["color"].SetValue(new Vector3(0, 0, 1));

			// set up some defaults for the shader program project and modelview matrices
			program["projection_matrix"].SetValue(Matrix4.CreatePerspectiveFieldOfView(0.45f, (float)width / height, 0.1f, 1000f));
			program["modelview_matrix"].SetValue(Matrix4.CreateTranslation(new Vector3(2, 2, -10)) * Matrix4.CreateRotation(new Vector3(1, -1, 0), 0.2f));

			// create a cube
			cube = OpenGL.Geometry.CreateCube(program, new Vector3(-1, -1, -1), new Vector3(1, 1, 1));

			Glut.glutMainLoop ();
		}

		private static void IdleFunc()
		{
			// clear the screen (both color and depth)
			Gl.Viewport(0, 0, width, height);
			Gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			// enable the shader program for our cube
			cube.Program.Use();
			cube.Draw();

			Glut.glutSwapBuffers();
		}

		private static void DisplayFunc()
		{
		}

		public static string fragmentShaderSource = @"
uniform vec3 color;
 
void main(void)
{
  gl_FragColor = vec4(color, 1);
}";

		public static string vertexShaderSource = @"
uniform mat4 projection_matrix;
uniform mat4 modelview_matrix;
 
attribute vec3 in_position;
 
void main(void)
{
  gl_Position = projection_matrix * modelview_matrix * vec4(in_position, 1);
}";
	}
}

