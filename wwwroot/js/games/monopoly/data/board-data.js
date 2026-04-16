// 36-square Monopoly board
// Types: start, property, chance, community, tax, free-parking, jail-visit
export const BOARD = [
    { id:0,  type:'start',         title:'GO 起點',       icon:'🏁', group:null,       price:0,   rent:[0,0,0,0] },
    { id:1,  type:'property',      title:'C# 基礎 I',     icon:'🔷', group:'brown',    price:200, rent:[25,50,100,200] },
    { id:2,  type:'property',      title:'C# 基礎 II',    icon:'🔷', group:'brown',    price:220, rent:[30,60,120,240] },
    { id:3,  type:'community',     title:'機會寶箱',       icon:'📦', group:null,       price:0,   rent:[0,0,0,0] },
    { id:4,  type:'property',      title:'變數與型別 I',   icon:'📝', group:'brown',    price:240, rent:[35,70,140,280] },
    { id:5,  type:'property',      title:'變數與型別 II',  icon:'📝', group:'brown',    price:260, rent:[40,80,160,320] },
    { id:6,  type:'tax',           title:'繳稅站',         icon:'💸', group:null,       price:150, rent:[0,0,0,0] },
    { id:7,  type:'property',      title:'HTML/CSS I',     icon:'🎨', group:'lightBlue', price:280, rent:[35,75,150,300] },
    { id:8,  type:'property',      title:'HTML/CSS II',    icon:'🎨', group:'lightBlue', price:300, rent:[40,80,160,320] },
    { id:9,  type:'chance',        title:'命運卡',         icon:'🃏', group:null,       price:0,   rent:[0,0,0,0] },
    { id:10, type:'property',      title:'JavaScript I',   icon:'🟨', group:'lightBlue', price:320, rent:[45,90,180,360] },
    { id:11, type:'property',      title:'JavaScript II',  icon:'🟨', group:'lightBlue', price:340, rent:[50,100,200,400] },
    { id:12, type:'property',      title:'JS DOM',         icon:'🌳', group:'lightBlue', price:350, rent:[50,100,200,400] },
    { id:13, type:'free-parking',  title:'免費停車',       icon:'🅿️', group:null,       price:0,   rent:[0,0,0,0] },
    { id:14, type:'property',      title:'SQL 基礎 I',     icon:'📘', group:'pink',     price:360, rent:[55,110,220,440] },
    { id:15, type:'property',      title:'SQL 基礎 II',    icon:'📘', group:'pink',     price:380, rent:[60,120,240,480] },
    { id:16, type:'community',     title:'機會寶箱',       icon:'📦', group:null,       price:0,   rent:[0,0,0,0] },
    { id:17, type:'property',      title:'LINQ I',         icon:'🔍', group:'pink',     price:400, rent:[65,130,260,520] },
    { id:18, type:'property',      title:'LINQ II',        icon:'🔍', group:'pink',     price:420, rent:[70,140,280,560] },
    { id:19, type:'property',      title:'ASP.NET I',      icon:'🌐', group:'orange',   price:440, rent:[70,140,280,560] },
    { id:20, type:'property',      title:'ASP.NET II',     icon:'🌐', group:'orange',   price:460, rent:[75,150,300,600] },
    { id:21, type:'property',      title:'ASP.NET III',    icon:'🌐', group:'orange',   price:480, rent:[80,160,320,640] },
    { id:22, type:'chance',        title:'命運卡',         icon:'🃏', group:null,       price:0,   rent:[0,0,0,0] },
    { id:23, type:'property',      title:'OOP 設計 I',     icon:'🏛️', group:'orange',   price:500, rent:[85,170,340,680] },
    { id:24, type:'property',      title:'OOP 設計 II',    icon:'🏛️', group:'orange',   price:520, rent:[90,180,360,720] },
    { id:25, type:'jail-visit',    title:'參觀監獄',       icon:'🔒', group:null,       price:0,   rent:[0,0,0,0] },
    { id:26, type:'property',      title:'async/await I',  icon:'⏳', group:'green',    price:540, rent:[90,180,360,720] },
    { id:27, type:'property',      title:'async/await II', icon:'⏳', group:'green',    price:560, rent:[95,190,380,760] },
    { id:28, type:'community',     title:'機會寶箱',       icon:'📦', group:null,       price:0,   rent:[0,0,0,0] },
    { id:29, type:'property',      title:'Docker I',       icon:'🐳', group:'green',    price:580, rent:[100,200,400,800] },
    { id:30, type:'property',      title:'Docker II',      icon:'🐳', group:'green',    price:600, rent:[100,200,400,800] },
    { id:31, type:'tax',           title:'伺服器維護費',   icon:'🔧', group:null,       price:200, rent:[0,0,0,0] },
    { id:32, type:'property',      title:'Security I',     icon:'🔒', group:'darkBlue', price:620, rent:[110,220,440,880] },
    { id:33, type:'property',      title:'Security II',    icon:'🔒', group:'darkBlue', price:640, rent:[120,240,480,960] },
    { id:34, type:'chance',        title:'命運卡',         icon:'🃏', group:null,       price:0,   rent:[0,0,0,0] },
    { id:35, type:'property',      title:'系統架構',       icon:'🏗️', group:'darkBlue', price:700, rent:[130,260,520,1040] },
];

// How many squares per side (for layout calculation)
// Bottom: 10, Right: 8, Top: 10, Left: 8 = 36
export const SIDES = { bottom: 10, right: 8, top: 10, left: 8 };
