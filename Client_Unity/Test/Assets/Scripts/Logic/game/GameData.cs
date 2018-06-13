﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Tetris
{
	public uint m_dwTetrisShape;
	public uint m_dwTetrisDirect;
	public uint m_dwTetrisColor;

	//坐标位置为左下角
	public uint m_dwPosX;
	public uint m_dwPosY;
}

[System.Serializable]
public class TetrisData
{
	public static readonly uint s_dwColumn = 12;
	public static readonly uint s_dwRow = 22;
	public static readonly uint s_dwUnit = 4;

	//形状的数量
	public const uint s_dwShapeCount = 7;
	//方向的数量
	public const uint s_dwDirectCount = 4;

	public static void SetSuspendTime(float fSuspendTime) { s_fSuspendTime = fSuspendTime; }
	public static float proSuspendTime { get { return s_fSuspendTime; } }
    static float s_fSuspendTime = 0.5f;

	// 7种方块的4旋转状态
	public static readonly uint[,,,] s_wTetrisTable = new uint[7, 4, 5, 4]
	{
		// I型 { 0x00F0, 0x2222, 0x00F0, 0x2222 },  第五行代表最下面的块的位置
		{
			{
				{ 0x0,  0xFFFFFFFF, 0x0,	0x0, },
				{ 0x0,  0xFFFFFFFF, 0x0,	0x0, },
				{ 0x0,  0xFFFFFFFF, 0x0,	0x0, },
				{ 0x0,  0xFFFFFFFF, 0x0,	0x0, },
				{ 0x0,  0xFFFFFFF0, 0x0,	0x0, },
			},
			{
				{ 0x0,		  0x0,		0x0,		0x0, },
				{ 0xFFFFFFFF,   0xFFFFFFFF, 0xFFFFFFFF, 0xFFFFFFFF, },
				{ 0x0,		  0x0,		0x0,		0x0, },
				{ 0x0,		  0x0,		0x0,		0x0, },
				{ 0xFFFFFFF2,   0xFFFFFFF2, 0xFFFFFFF2,	 0xFFFFFFF2, },
			},
			{
				{ 0x0,  0xFFFFFFFF, 0x0,	0x0, },
				{ 0x0,  0xFFFFFFFF, 0x0,	0x0, },
				{ 0x0,  0xFFFFFFFF, 0x0,	0x0, },
				{ 0x0,  0xFFFFFFFF, 0x0,	0x0, },
				{ 0x0,  0xFFFFFFF0, 0x0,	0x0, },
			},
			{
				{ 0x0,		  0x0,		0x0,		0x0, },
				{ 0xFFFFFFFF,   0xFFFFFFFF, 0xFFFFFFFF, 0xFFFFFFFF, },
				{ 0x0,		  0x0,		0x0,		0x0, },
				{ 0x0,		  0x0,		0x0,		0x0, },
				{ 0xFFFFFFF2,   0xFFFFFFF2, 0xFFFFFFF2,	 0xFFFFFFF2, },
			},
		},
		// T型 { 0x0072, 0x0262, 0x0270, 0x0232 },  第五行代表最下面的块的位置
		{
			{
				{0x0,   0x0,	0xFFFFFFFF, 0x0, },
				{0x0,   0x0,	0xFFFFFFFF, 0xFFFFFFFF, },
				{0x0,   0x0,	0xFFFFFFFF, 0x0, },
				{0x0,   0x0,	0x0,		0x0, },
				{0x0,   0x0,	0xFFFFFFF1, 0xFFFFFFF2, },
			},
			{
				{0x0,   0x0,		0x0,		0x0, },
				{0x0,   0xFFFFFFFF, 0xFFFFFFFF, 0xFFFFFFFF, },
				{0x0,   0x0,		0xFFFFFFFF, 0x0, },
				{0x0,   0x0,		0x0,		0x0, },
				{0x0,	0xFFFFFFF2, 0xFFFFFFF1, 0xFFFFFFF2, },
			},
			{
				{0x0,   0x0,		0xFFFFFFFF, 0x0, },
				{0x0,   0xFFFFFFFF, 0xFFFFFFFF, 0x0, },
				{0x0,   0x0,		0xFFFFFFFF, 0x0, },
				{0x0,   0x0,		0x0,		0x0, },
				{0x0,	0xFFFFFFF2, 0xFFFFFFF1,	0x0, },
			},
			{
				{0x0,   0x0,		0xFFFFFFFF, 0x0, },
				{0x0,   0xFFFFFFFF, 0xFFFFFFFF, 0xFFFFFFFF, },
				{0x0,   0x0,		0x0,		0x0, },
				{0x0,   0x0,		0x0,		0x0, },
				{0x0,   0xFFFFFFF2, 0xFFFFFFF2, 0xFFFFFFF2, },
			},
		},
		//L型 //{ 0x0223, 0x0074, 0x0622, 0x0170 },  第五行代表最下面的块的位置
		{
			{
				{0x0,   0x0,		0x0,		0xFFFFFFFF, },
				{0x0,   0xFFFFFFFF, 0xFFFFFFFF, 0xFFFFFFFF, },
				{0x0,   0x0,		0x0,		0x0, },
				{0x0,   0x0,		0x0,		0x0, },
				{0x0,   0xFFFFFFF2, 0xFFFFFFF2, 0xFFFFFFF2, },
			},
			{
				{0x0,   0x0,	0xFFFFFFFF, 0x0, },
				{0x0,   0x0,	0xFFFFFFFF, 0x0, },
				{0x0,   0x0,	0xFFFFFFFF, 0xFFFFFFFF, },
				{0x0,   0x0,	0x0,		0x0, },
				{0x0,	0x0,    0xFFFFFFF1, 0xFFFFFFF1, },
			},
			{
				{0x0,   0x0,		0x0,		0x0, },
				{0x0,   0xFFFFFFFF,	0xFFFFFFFF,	0xFFFFFFFF, },
				{0x0,   0xFFFFFFFF,	0x0,		0x0, },
				{0x0,   0x0,		0x0,		0x0, },
				{0x0,   0xFFFFFFF1, 0xFFFFFFF2, 0xFFFFFFF2, },
			},
			{
				{0x0,   0xFFFFFFFF,	0xFFFFFFFF,	0x0, },
				{0x0,   0x0,		0xFFFFFFFF,	0x0, },
				{0x0,   0x0,		0xFFFFFFFF,	0x0, },
				{0x0,   0x0,		0x0,		0x0, },
				{0x0,   0xFFFFFFF3, 0xFFFFFFF1,	0x0, },
			},
		},
		//J型 { 0x0226, 0x0470, 0x0322, 0x0071 },  第五行代表最下面的块的位置
		{
			{
				{0x0,   0x0,		0x0,		0x0, },
				{0x0,   0xFFFFFFFF,	0xFFFFFFFF,	0xFFFFFFFF, },
				{0x0,   0x0,		0x0,		0xFFFFFFFF, },
				{0x0,   0x0,		0x0,		0x0, },
				{0x0,   0xFFFFFFF2, 0xFFFFFFF2, 0xFFFFFFF1, },
			},
			{
				{0x0,   0x0,		0xFFFFFFFF,	0x0, },
				{0x0,   0x0,		0xFFFFFFFF,	0x0, },
				{0x0,   0xFFFFFFFF,	0xFFFFFFFF,	0x0, },
				{0x0,   0x0,		0x0,		0x0, },
				{0x0,	0xFFFFFFF1, 0xFFFFFFF1,	0x0, },
			},
			{
				{0x0,   0xFFFFFFFF,	0x0,		0x0, },
				{0x0,   0xFFFFFFFF,	0xFFFFFFFF,	0xFFFFFFFF, },
				{0x0,   0x0,		0x0,		0x0, },
				{0x0,   0x0,		0x0,		0x0, },
				{0x0,   0xFFFFFFF2, 0xFFFFFFF2, 0xFFFFFFF2, },
			},
			{
				{0x0,   0x0,	0xFFFFFFFF,	0xFFFFFFFF, },
				{0x0,   0x0,	0xFFFFFFFF,	0x0, },
				{0x0,   0x0,	0xFFFFFFFF,	0x0, },
				{0x0,   0x0,	0x0,		0x0, },
				{0x0,	0x0,    0xFFFFFFF1, 0xFFFFFFF3, },
			},
		},
		//Z型 { 0x0063, 0x0264, 0x0063, 0x0264 },  第五行代表最下面的块的位置
		{
			{
				{0x0,   0x0,	0x0,		0xFFFFFFFF, },
				{0x0,   0x0,	0xFFFFFFFF,	0xFFFFFFFF, },
				{0x0,   0x0,	0xFFFFFFFF,	0x0, },
				{0x0,   0x0,	0x0,		0x0, },
				{0x0,	0x0,    0xFFFFFFF1, 0xFFFFFFF2, },
			},
			{
				{0x0,   0x0,		0x0,		0x0, },
				{0x0,   0xFFFFFFFF,	0xFFFFFFFF,	0x0, },
				{0x0,   0x0,		0xFFFFFFFF,	0xFFFFFFFF, },
				{0x0,   0x0,		0x0,		0x0, },
				{0x0,   0xFFFFFFF2, 0xFFFFFFF1, 0xFFFFFFF1, },
			},
			{
				{0x0,   0x0,	0x0,		0xFFFFFFFF, },
				{0x0,   0x0,	0xFFFFFFFF,	0xFFFFFFFF, },
				{0x0,   0x0,	0xFFFFFFFF,	0x0, },
				{0x0,   0x0,	0x0,		0x0, },
				{0x0,	0x0,    0xFFFFFFF1, 0xFFFFFFF2, },
			},
			{
				{0x0,   0x0,		0x0,		0x0, },
				{0x0,   0xFFFFFFFF,	0xFFFFFFFF,	0x0, },
				{0x0,   0x0,		0xFFFFFFFF,	0xFFFFFFFF, },
				{0x0,   0x0,		0x0,		0x0, },
				{0x0,   0xFFFFFFF2, 0xFFFFFFF1, 0xFFFFFFF1, },
			},
		},
		//S型 { 0x006C, 0x0462, 0x006C, 0x0462 },  第五行代表最下面的块的位置
		{
			{
				{0x0,   0x0,	0x0,		0x0, },
				{0x0,   0x0,	0xFFFFFFFF,	0x0, },
				{0x0,   0x0,	0xFFFFFFFF,	0xFFFFFFFF, },
				{0x0,   0x0,	0x0,		0xFFFFFFFF, },
				{0x0,	0x0,    0xFFFFFFF1, 0xFFFFFFF0, },
			},
			{
				{0x0,   0x0,		0x0,		0x0, },
				{0x0,   0x0,		0xFFFFFFFF,	0xFFFFFFFF, },
				{0x0,   0xFFFFFFFF,	0xFFFFFFFF,	0x0, },
				{0x0,   0x0,		0x0,		0x0, },
				{0x0,   0xFFFFFFF1, 0xFFFFFFF1, 0xFFFFFFF2, },
			},
			{
				{0x0,   0x0,	0x0,		0x0, },
				{0x0,   0x0,	0xFFFFFFFF,	0x0, },
				{0x0,   0x0,	0xFFFFFFFF,	0xFFFFFFFF, },
				{0x0,   0x0,	0x0,		0xFFFFFFFF, },
				{ 0x0,	0x0,    0xFFFFFFF1, 0xFFFFFFF0, },
			},
			{
				{0x0,   0x0,		0x0,		0x0, },
				{0x0,   0x0,		0xFFFFFFFF,	0xFFFFFFFF, },
				{0x0,   0xFFFFFFFF,	0xFFFFFFFF,	0x0, },
				{0x0,   0x0,		0x0,		0x0, },
				{0x0,   0xFFFFFFF1, 0xFFFFFFF1, 0xFFFFFFF2, },
			},
		},
		//O型 { 0x0660, 0x0660, 0x0660, 0x0660 }  第五行代表最下面的块的位置
		{
			{
				{0x0,   0x0,		0x0,		0x0, },
				{0x0,   0xFFFFFFFF,	0xFFFFFFFF,	0x0, },
				{0x0,   0xFFFFFFFF,	0xFFFFFFFF,	0x0, },
				{0x0,   0x0,		0x0,		0x0, },
				{0x0,   0xFFFFFFF1,	0xFFFFFFF1,	0x0, },
			},{
				{0x0,   0x0,        0x0,        0x0, },
				{0x0,   0xFFFFFFFF, 0xFFFFFFFF, 0x0, },
				{0x0,   0xFFFFFFFF, 0xFFFFFFFF, 0x0, },
				{0x0,   0x0,        0x0,        0x0, },
				{0x0,   0xFFFFFFF1, 0xFFFFFFF1, 0x0, },
			},
			{
				{0x0,   0x0,        0x0,        0x0, },
				{0x0,   0xFFFFFFFF, 0xFFFFFFFF, 0x0, },
				{0x0,   0xFFFFFFFF, 0xFFFFFFFF, 0x0, },
				{0x0,   0x0,        0x0,        0x0, },
				{0x0,   0xFFFFFFF1, 0xFFFFFFF1, 0x0, },
			},
			{
				{0x0,   0x0,        0x0,        0x0, },
				{0x0,   0xFFFFFFFF, 0xFFFFFFFF, 0x0, },
				{0x0,   0xFFFFFFFF, 0xFFFFFFFF, 0x0, },
				{0x0,   0x0,        0x0,        0x0, },
				{0x0,   0xFFFFFFF1, 0xFFFFFFF1, 0x0, },
			},
		},
	};

	void Sync()
	{
	}

	//所有的方块 每个元素代表一种颜色
	public uint[,] m_dwTetrisPool = new uint[s_dwRow, s_dwColumn];
	//当前方块
	//uint[,] m_dwCurrBlock = new uint[s_dwUnit, s_dwUnit];
	////下一个方块
	//uint[,] m_dwNextBlock = new uint[s_dwUnit, s_dwUnit];

	public Tetris m_oCurrentTetris = new Tetris();

	public Tetris m_oNextTetris = new Tetris();
}

public class UserTetrisData : TetrisData
{
	public bool CheckUnderTetris(uint dwRow, uint dwCol)
	{
		uint dwColor = (m_dwTetrisPool[dwRow, dwCol] & 0xFFFFFF00);
		return dwColor != 0;
	}

	public bool CheckDownTetris()
	{
		//检查方块还能不能继续下降
		for (uint i = 0; i < s_dwDirectCount; i++)
		{
			uint dwBlockInfo = s_wTetrisTable[m_oCurrentTetris.m_dwTetrisShape, m_oCurrentTetris.m_dwTetrisDirect, 4, i];
			if (dwBlockInfo == 0)
			{
				continue;
			}

			if (CheckUnderTetris(m_oCurrentTetris.m_dwPosX + i, m_oCurrentTetris.m_dwPosY + (dwBlockInfo & 0x0000000F) -1))
			{
				return true;
			}
		}
		return false;
	}

	public void DownTetris()
	{
		//方块的位置初始化于中间的方格
		//坐标是方块的左下角
		//下降的时候 先要找出当前方块最下面的几个块的坐标
	}

	public void Update(float fDeltaTime)
	{
	}
}

