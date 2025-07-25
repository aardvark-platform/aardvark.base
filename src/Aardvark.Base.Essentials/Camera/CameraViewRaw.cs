/*
    Copyright 2006-2025. The Aardvark Platform Team.

        https://aardvark.graphics

    Licensed under the Apache License, Version 2.0 (the "License");
    you may not use this file except in compliance with the License.
    You may obtain a copy of the License at

        http://www.apache.org/licenses/LICENSE-2.0

    Unless required by applicable law or agreed to in writing, software
    distributed under the License is distributed on an "AS IS" BASIS,
    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
    See the License for the specific language governing permissions and
    limitations under the License.
*/
namespace Aardvark.Base;

public class CameraViewRaw : ICameraView
{
    private Trafo3d m_trafo = Trafo3d.ViewTrafo(V3d.Zero, V3d.XAxis, V3d.ZAxis, -V3d.YAxis);
    private readonly EventSource<Trafo3d> m_trafoChanges;

    public CameraViewRaw()
    {
        m_trafoChanges = EventSource.Create(m_trafo);
    }

    /// <summary>
    /// Gets or sets view trafo, which transforms world space into camera space.
    /// 
    /// In camera space the camera is placed at the origin looking
    /// down the negative z-axis, with the y-axis pointing upwards
    /// and the x-axis pointing to the right.
    /// </summary>
    public Trafo3d ViewTrafo
    {
        get { return m_trafo; }
        set { m_trafo = value; m_trafoChanges.Emit(m_trafo); }
    }

    /// <summary>
    /// Changes of ViewTrafo.
    /// </summary>
    public IEvent<Trafo3d> ViewTrafos => m_trafoChanges;

    /// <summary>
    /// Gets or sets camera location in world space (origin in camera space).
    /// </summary>
    public V3d Location
    {
        get { return m_trafo.Backward.C3.XYZ; }
        set { m_trafo = Trafo3d.ViewTrafo(value, Right, Up, -Forward); m_trafoChanges.Emit(m_trafo); }
    }

    /// <summary>
    /// Gets or sets camera right vector in world space (+x in camera space).
    /// </summary>
    public V3d Right
    {
        get { return m_trafo.Backward.C0.XYZ; }
        set { m_trafo = Trafo3d.ViewTrafo(Location, value, Up, -Forward); m_trafoChanges.Emit(m_trafo); }
    }

    /// <summary>
    /// Gets or sets camera up vector in world space (+y in camera space).
    /// </summary>
    public V3d Up
    {
        get { return m_trafo.Backward.C1.XYZ; }
        set { m_trafo = Trafo3d.ViewTrafo(Location, Right, value, -Forward); m_trafoChanges.Emit(m_trafo); }
    }

    /// <summary>
    /// Gets or sets camera forward direction in world space (-z in camera space).
    /// </summary>
    public V3d Forward
    {
        get { return -m_trafo.Backward.C2.XYZ; }
        set { m_trafo = Trafo3d.ViewTrafo(Location, Right, Up, -value); m_trafoChanges.Emit(m_trafo); }
    }
    
    /// <summary>
    /// Sets location and axes in a single transaction.
    /// </summary>
    public void Set(V3d location, V3d right, V3d up, V3d forward)
    {
        m_trafo = Trafo3d.ViewTrafo(location, right, up, -forward);
        m_trafoChanges.Emit(m_trafo);
    }
}
