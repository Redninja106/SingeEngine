﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Singe.Rendering
{
    public interface IShader : IGraphicsResource
    {
        ShaderReflection GetReflector();
    }
}
