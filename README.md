# Jump Race Master Pro - Dual Lane

.NET 10 Blazor WebAssembly 版本

## 运行方式

```bash
# 需要安装 .NET 10 SDK
dotnet restore
dotnet run
```

浏览器访问提示的地址（通常是 `https://localhost:7xxx`）。

## 操作说明

**桌面端**
- 按键盘 `1` → 左侧选手（Girls）跳跃
- 按键盘 `2` → 右侧选手（Boys）跳跃

**移动端**
- 触摸底部左侧大按钮 → 左侧选手跳跃
- 触摸底部右侧大按钮 → 右侧选手跳跃
- 建议横屏游玩

- 点击右下角 ❤️ 打开设置面板

## 项目结构

```
JumpRace.Blazor/
├── Models/                 # 数据模型
├── Services/               # 游戏逻辑、配置、音效服务
├── Components/
│   ├── Game/               # 赛道、角色、进度条、胜利遮罩
│   ├── Settings/           # 设置弹窗
│   └── Layout/
├── Pages/
│   └── Index.razor         # 主页面
└── wwwroot/
    ├── css/app.css
    ├── js/game-interop.js  # Canvas / Web Audio 互操作
    └── img/                # 图片与背景视频
```

## 主要功能

- 双人跳跃竞速
- 自定义玩家名称、颜色、头像、跳跃图
- 场景粒子特效（雨、雪、星星等）
- 背景视频轮播
- 胜场统计（localStorage 持久化）
- 胜利动画 + 彩纸
- **PWA 支持**（Service Worker + Manifest）
- **移动端触摸按钮**（自动适配手机/平板）

## PWA / 安装为应用

项目已加入 Service Worker 和 Web Manifest。

发布后：
```bash
dotnet publish -c Release
```

用 HTTPS 打开发布后的站点，浏览器会出现「安装」按钮，安装后可以：
- 像桌面应用一样独立窗口打开（无地址栏）
- 支持离线缓存
- 添加到开始菜单 / 桌面 / 手机主屏幕

注意：Service Worker 不能完全脱离浏览器，但安装后的 PWA 可以以独立窗口运行。

## 后续可优化

1. 把大量 tile 改为 Canvas 绘制（提升性能）
2. 发布时开启 AOT 减小体积
3. 准备更高分辨率的 PWA 图标（192 / 512）
