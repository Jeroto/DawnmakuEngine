using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using DawnmakuEngine.Elements;
using OpenTK;
using OpenTK.Mathematics;

namespace DawnmakuEngine.Data
{
    public class BackgroundSection
    {
        public float secLength = 0;
        public Vector3 offset = Vector3.Zero;
        public List<TexturedModel[]> models = new List<TexturedModel[]>();
        public List<int> parentInds = new List<int>();
        public List<Vector3> modelPos = new List<Vector3>();
        public List<Vector3> modelRot = new List<Vector3>();
        public List<Vector3> modelScale = new List<Vector3>();
        public List<List<Element>> elements = new List<List<Element>>();

        public Entity SpawnSegment(Vector3 extraOffset)
        {
            Vector3 rotDeg = new Vector3();
            Entity newSeg = new Entity("Segment", offset + extraOffset), newModel;
            int i, e, modelCount = models.Count, rand;
            for (i = 0; i < modelCount; i++)
            {
                rotDeg.X = MathHelper.DegreesToRadians(modelRot[i].X);
                rotDeg.Y = MathHelper.DegreesToRadians(modelRot[i].Y);
                rotDeg.Z = MathHelper.DegreesToRadians(modelRot[i].Z);
                newModel = new Entity("Model", (parentInds[i] < i && parentInds[i] != -1) ? newSeg.GetChild(parentInds[i]) : newSeg, modelPos[i], modelRot[i], modelScale[i]);
                rand = GameMaster.Random(0, models[i].Length);
                if (models[i][rand] != null)
                    newModel.AddElement(new MeshRenderer(models[i][rand], 0, OpenTK.Graphics.ES30.BufferUsageHint.DynamicDraw, models[i][rand].shader));
                for (e = 0; e < elements[i].Count; e++)
                    newModel.AddElement(elements[i][e].Clone());
                newModel.SetWorldPosition();
            }
            return newSeg;
        }
        public Entity SpawnSegmentLocal(Entity obj, Vector3 extraOffset)
        {
            Vector3 rotDeg = new Vector3();
            Entity newSeg = new Entity("Segment", obj), newModel;
            newSeg.LocalPosition = offset + extraOffset;
            int i, e, modelCount = models.Count, rand;
            for (i = 0; i < modelCount; i++)
            {
                rotDeg.X = MathHelper.DegreesToRadians(modelRot[i].X);
                rotDeg.Y = MathHelper.DegreesToRadians(modelRot[i].Y);
                rotDeg.Z = MathHelper.DegreesToRadians(modelRot[i].Z);
                newModel = new Entity("Model", (parentInds[i] < i && parentInds[i] != -1) ? newSeg.GetChild(parentInds[i]) : newSeg, modelPos[i], rotDeg, modelScale[i]);
                rand = GameMaster.Random(0, models[i].Length);
                if(models[i][rand] != null)
                    newModel.AddElement(new MeshRenderer(models[i][rand], 0, OpenTK.Graphics.ES30.BufferUsageHint.DynamicDraw, models[i][rand].shader));
                for (e = 0; e < elements[i].Count; e++)
                    newModel.AddElement(elements[i][e].Clone());
                newModel.SetWorldPosition();
            }
            return newSeg;
        }
    }
}
