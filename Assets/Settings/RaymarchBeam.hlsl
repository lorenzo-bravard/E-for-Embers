void RaymarchBeam_float(
    float3 LocalPos, 
    float3 LocalViewDir, 
    float3 LightDir, 
    float Density, 
    float Softness,
    out float OutAlpha)
{
    // 1. INITIALISATION DE SECURITÉ (Obligatoire)
    OutAlpha = 0.0; 

    // 2. Définition de la boite
    float3 boxMin = float3(-0.5, -0.5, -0.5);
    float3 boxMax = float3(0.5, 0.5, 0.5);

    // 3. Intersection
    float3 invDir = 1.0 / (LocalViewDir + 1e-5);
    float3 tMin = (boxMin - LocalPos) * invDir;
    float3 tMax = (boxMax - LocalPos) * invDir;
    float3 t1 = min(tMin, tMax);
    float3 t2 = max(tMin, tMax);
    float tNear = max(max(t1.x, t1.y), t1.z);
    float tFar = min(min(t2.x, t2.y), t2.z);

    // 4. Vérification (Sans utiliser 'return' pour éviter le bug)
    // Si on touche la boite, on fait le calcul. Sinon, OutAlpha reste à 0.
    if (tNear <= tFar && tFar >= 0.0) 
    {
        float tStart = max(0.0, tNear);
        int steps = 20;
        float stepSize = (tFar - tStart) / float(steps);
        float currentT = tStart;
        float accumDensity = 0.0;

        for(int i = 0; i < steps; i++)
        {
            float3 currentPos = LocalPos + LocalViewDir * currentT;

            // Distance Point-Ligne
            float3 rayPoint = LightDir * dot(currentPos, LightDir);
            float dist = length(currentPos - rayPoint);

            float beam = smoothstep(Softness, 0.0, dist);
            
            accumDensity += beam * stepSize * Density;
            currentT += stepSize;
        }
        
        OutAlpha = saturate(accumDensity);
    }
}