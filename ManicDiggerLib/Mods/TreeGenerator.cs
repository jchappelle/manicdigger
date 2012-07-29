﻿using System;
using System.Collections.Generic;
using System.Text;
using LibNoise;

namespace ManicDigger.Mods
{
    public class TreeGenerator : IMod
    {
        public void PreStart(ModManager m)
        {
            m.RequireMod("Default");
        }
        public void Start(ModManager m)
        {
            this.m = m;
            TileIdGrass = m.GetBlockId("Grass");
            TileIdTreeTrunk = m.GetBlockId("TreeTrunk");
            TileIdLeaves = m.GetBlockId("Leaves");
            TileIdApples = m.GetBlockId("Apples");
            m.RegisterPopulateChunk(PopulateChunk);
        }
        ModManager m;

        int treeCount = 20;
        
        // ##### TREE GEN #####
        Billow treenoise = new Billow();
        // ##### END TREE GEN ####

        int TileIdGrass;
        int TileIdTreeTrunk;
        int TileIdLeaves;
        int TileIdApples;

        void Init()
        {
            int Seed = m.GetSeed();
            //###### TREE GEN #####
            treenoise.Seed = (Seed + 2);
            treenoise.OctaveCount = (6);
            treenoise.Frequency = (1.0 / 180.0);
            treenoise.Lacunarity = ((treeCount / 20.0) * (treeCount / 20.0) * 2.0);
            //###### END TREE GEN #######
        }

        Random _rnd = new Random();

        void PopulateChunk(int x, int y, int z)
        {
            x *= m.GetChunkSize();
            y *= m.GetChunkSize();
            z *= m.GetChunkSize();
            //forests
            //if (Math.Abs(treenoise.GetValue(x, 0, y)) >= 0.9)
            double count = treenoise.GetValue(x, 0, y) * 1000;
            {
                count = System.Math.Min(count, 300);
                MakeSmallTrees(x, y, z, m.GetChunkSize(), _rnd, (int)count);
            }
            //random trees
            MakeSmallTrees(x, y, z, m.GetChunkSize(), _rnd, treeCount + 10 - (10 - treeCount / 10));
        }

        void MakeSmallTrees(int cx, int cy, int cz, int chunksize, Random rnd, int count)
        {
            int chooseTreeType;
            for (int i = 0; i < count; i++)
            {
                int x = cx + rnd.Next(chunksize);
                int y = cy + rnd.Next(chunksize);
                int z = cz + rnd.Next(chunksize);
                if (!m.IsValidPos(x, y, z) || m.GetBlock(x, y, z) != TileIdGrass)
                {
                    continue;
                }
                chooseTreeType = rnd.Next(0, 3);
                switch (chooseTreeType)
                {
                    case 0: MakeTreeType1(x, y, z, rnd); break;
                    case 1: MakeTreeType2(x, y, z, rnd); break;
                    case 2: MakeTreeType3(x, y, z, rnd); break;
                };
            }
        }

        void MakeTreeType1(int x, int y, int z, Random rnd)
        {
            int treeHeight = rnd.Next(8, 12);
            int xx = 0;
            int yy = 0;
            int dir = 0;

            for (int i = 0; i < treeHeight; i++)
            {
                SetBlock(x, y, z + i, TileIdTreeTrunk);
                if (i == treeHeight - 4)
                {
                    SetBlock(x + 1, y, z + i, TileIdTreeTrunk);
                    SetBlock(x - 1, y, z + i, TileIdTreeTrunk);
                    SetBlock(x, y + 1, z + i, TileIdTreeTrunk);
                    SetBlock(x, y - 1, z + i, TileIdTreeTrunk);
                }

                if (i == treeHeight - 3)
                {
                    for (int j = 1; j < 9; j++)
                    {
                        dir += 45;
                        for (int k = 1; k < 4; k++)
                        {
                            int length = dir % 90 == 0 ? k : (int)(k / 2);
                            xx = length * (int)System.Math.Round(System.Math.Cos(dir * System.Math.PI / 180));
                            yy = length * (int)System.Math.Round(System.Math.Sin(dir * System.Math.PI / 180));

                            SetBlock(x + xx, y + yy, z + i, TileIdTreeTrunk);
                            SetBlockIfEmpty(x + xx, y + yy, z + i + 1, TileIdLeaves);

                            SetBlockIfEmpty(x + xx + 1, y + yy, z + i, TileIdLeaves);
                            SetBlockIfEmpty(x + xx - 1, y + yy, z + i, TileIdLeaves);
                            SetBlockIfEmpty(x + xx, y + yy + 1, z + i, TileIdLeaves);
                            SetBlockIfEmpty(x + xx, y + yy - 1, z + i, TileIdLeaves);
                        }
                    }
                }
                if (i == treeHeight - 1)
                {
                    for (int j = 1; j < 9; j++)
                    {
                        dir += 45;
                        for (int k = 1; k < 3; k++)
                        {
                            int length = dir % 90 == 0 ? k : (int)(k / 2);
                            xx = length * (int)System.Math.Round(System.Math.Cos(dir * System.Math.PI / 180));
                            yy = length * (int)System.Math.Round(System.Math.Sin(dir * System.Math.PI / 180));

                            SetBlock(x + xx, y + yy, z + i, TileIdTreeTrunk);
                            SetBlockIfEmpty(x + xx, y + yy, z + i + 1, TileIdLeaves);

                            SetBlockIfEmpty(x + xx + 1, y + yy, z + i, TileIdLeaves);
                            SetBlockIfEmpty(x + xx - 1, y + yy, z + i, TileIdLeaves);
                            SetBlockIfEmpty(x + xx, y + yy + 1, z + i, TileIdLeaves);
                            SetBlockIfEmpty(x + xx, y + yy - 1, z + i, TileIdLeaves);
                        }
                    }
                }
            }
        }

        void MakeTreeType2(int x, int y, int z, Random rnd)
        {
            int treeHeight = rnd.Next(4, 6);
            int xx = 0;
            int yy = 0;
            int dir = 0;
            float chanceToAppleTree = 0.1f;
            for (int i = 0; i < treeHeight; i++)
            {
                SetBlock(x, y, z + i, TileIdTreeTrunk);
                if (i == treeHeight - 1)
                {
                    for (int j = 1; j < 9; j++)
                    {
                        dir += 45;
                        for (int k = 1; k < 2; k++)
                        {
                            int length = dir % 90 == 0 ? k : (int)(k / 2);
                            xx = length * (int)System.Math.Round(System.Math.Cos(dir * System.Math.PI / 180));
                            yy = length * (int)System.Math.Round(System.Math.Sin(dir * System.Math.PI / 180));

                            SetBlock(x + xx, y + yy, z + i, TileIdTreeTrunk);
                            if (chanceToAppleTree < rnd.NextDouble())
                            {
                                SetBlockIfEmpty(x + xx, y + yy, z + i + 1, TileIdLeaves);
                                SetBlockIfEmpty(x + xx + 1, y + yy, z + i, TileIdLeaves);
                                SetBlockIfEmpty(x + xx - 1, y + yy, z + i, TileIdLeaves);
                                SetBlockIfEmpty(x + xx, y + yy + 1, z + i, TileIdLeaves);
                                SetBlockIfEmpty(x + xx, y + yy - 1, z + i, TileIdLeaves);
                            }
                            else
                            {
                                float appleChance = 0.4f;
                                int tile;
                                tile = rnd.NextDouble() < appleChance ? TileIdApples : TileIdLeaves; SetBlockIfEmpty(x + xx, y + yy, z + i + 1, tile);
                                tile = rnd.NextDouble() < appleChance ? TileIdApples : TileIdLeaves; SetBlockIfEmpty(x + xx + 1, y + yy, z + i, tile);
                                tile = rnd.NextDouble() < appleChance ? TileIdApples : TileIdLeaves; SetBlockIfEmpty(x + xx - 1, y + yy, z + i, tile);
                                tile = rnd.NextDouble() < appleChance ? TileIdApples : TileIdLeaves; SetBlockIfEmpty(x + xx, y + yy + 1, z + i, tile);
                                tile = rnd.NextDouble() < appleChance ? TileIdApples : TileIdLeaves; SetBlockIfEmpty(x + xx, y + yy - 1, z + i, tile);
                            }
                        }
                    }
                }
            }
        }

        void MakeTreeType3(int x, int y, int z, Random rnd)
        {
            int treeHeight = rnd.Next(6, 9);
            int xx = 0;
            int yy = 0;
            int dir = 0;
            for (int i = 0; i < treeHeight; i++)
            {
                SetBlock(x, y, z + i, TileIdTreeTrunk);
                if (i % 3 == 0 && i > 3)
                {
                    for (int j = 1; j < 9; j++)
                    {
                        dir += 45;
                        for (int k = 1; k < 2; k++)
                        {
                            int length = dir % 90 == 0 ? k : (int)(k / 2);
                            xx = length * (int)System.Math.Round(System.Math.Cos(dir * System.Math.PI / 180));
                            yy = length * (int)System.Math.Round(System.Math.Sin(dir * System.Math.PI / 180));

                            SetBlock(x + xx, y + yy, z + i, TileIdTreeTrunk);
                            SetBlockIfEmpty(x + xx, y + yy, z + i + 1, TileIdLeaves);

                            SetBlockIfEmpty(x + xx + 1, y + yy, z + i, TileIdLeaves);
                            SetBlockIfEmpty(x + xx - 1, y + yy, z + i, TileIdLeaves);
                            SetBlockIfEmpty(x + xx, y + yy + 1, z + i, TileIdLeaves);
                            SetBlockIfEmpty(x + xx, y + yy - 1, z + i, TileIdLeaves);
                        }
                    }
                }
                if (i % 3 == 2 && i > 3)
                {
                    dir = 45;
                    for (int j = 1; j < 9; j++)
                    {
                        dir += 45;
                        for (int k = 1; k < 3; k++)
                        {
                            int length = dir % 90 == 0 ? k : (int)(k / 2);
                            xx = length * (int)System.Math.Round(System.Math.Cos(dir * System.Math.PI / 180));
                            yy = length * (int)System.Math.Round(System.Math.Sin(dir * System.Math.PI / 180));

                            SetBlock(x + xx, y + yy, z + i, TileIdTreeTrunk);
                            SetBlockIfEmpty(x + xx, y + yy, z + i + 1, TileIdLeaves);

                            SetBlockIfEmpty(x + xx + 1, y + yy, z + i, TileIdLeaves);
                            SetBlockIfEmpty(x + xx - 1, y + yy, z + i, TileIdLeaves);
                            SetBlockIfEmpty(x + xx, y + yy + 1, z + i, TileIdLeaves);
                            SetBlockIfEmpty(x + xx, y + yy - 1, z + i, TileIdLeaves);
                        }
                    }
                }
                SetBlockIfEmpty(x, y, z + treeHeight, TileIdLeaves);
            }
        }

        void SetBlock(int x, int y, int z, int blocktype)
        {
            if (m.IsValidPos(x, y, z))
            {
                m.SetBlock(x, y, z, blocktype);
            }
        }

        void SetBlockIfEmpty(int x, int y, int z, int blocktype)
        {
            if (m.IsValidPos(x, y, z) && m.GetBlock(x, y, z) == 0)
            {
                m.SetBlock(x, y, z, blocktype);
            }
        }
    }
}
