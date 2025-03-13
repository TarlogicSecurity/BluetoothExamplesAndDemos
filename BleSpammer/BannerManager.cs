using static System.Net.Mime.MediaTypeNames;

namespace BleSpammer {

    internal class BannerManager {

        //-------------------------------------------------------------------------------------------------------------------------------
        private static BannerManager m_Instance = null;
        public static BannerManager Instance { get { if (m_Instance == null) m_Instance = new BannerManager(); return m_Instance; } }
        //-------------------------------------------------------------------------------------------------------------------------------

        //-------------------------------------------------------------------------------------------------------------------------------
        private class ColoredLine {
            public readonly List<ColoredText> Segments = new List<ColoredText>();
            public override string ToString() => string.Join("",Segments);
        }
        private class ColoredText {
            public byte R;
            public byte G;
            public byte B;

            public string Text;

            public ColoredText(byte r,byte g,byte b,string text) {
                R = r;
                G = g;
                B = b;
                Text = text;
            }

            public override string ToString() { return $"\x1b[38;2;{R};{G};{B}m{Text}\x1b[0m"; }
        }
        //-------------------------------------------------------------------------------------------------------------------------------

        //-------------------------------------------------------------------------------------------------------------------------------
        internal static void Show() {

            string[] textArt = new string[]  {
                "  ___ _        ___",
                " | _ ) |___   / __| ____  ____ _ __   _ __   ___  _",
                " | _ \\ / -_)  \\__ \\| '_ \\) _  | '  \\ | '  \\ / -_)| '_| ",
                " |___/_\\___|  |___/| .__/\\__,_|_|_|_||_|_|_|\\___||_|   ",
                "                   |_|        v1.0 - Tarlogic Security"
            };

            string[] flameArt = new string[] {
                "               (                                       ",
                "   (  (        )\\ )                                    ",
                " ( )\\ )\\  (   (()/(          )    )      )     (  (    ",
                " )((_|(_)))\\   /(_))  )  ( /(   (      (     ))\\ )(   ",
                "((_)_ _ /((_) (_))  /(/(  )(_))  )\\  '  )\\  '/((_|()\\  ",
                "       (_))        ((_)\\((_)_ _((_)) _((_))(_)) _((_)",
                "                                    ()     ()          "
            };


            int totalLines = 9;

            var flameLines  = Enumerable.Range(0, totalLines).Select(_ => new ColoredLine()).ToArray();
            var textLines   = Enumerable.Range(0, totalLines).Select(_ => new ColoredLine()).ToArray();

            int maxWidth = flameArt.Concat(textArt).Max(line => line.Length);

            var flameOffsets = new (int dr, int dg, int db)[maxWidth];

            for (int c = 0; c < maxWidth; c++) {
                int dr = Random.Shared.Next(-30, 31);
                int dg = Random.Shared.Next(-50, 51);
                int db = Random.Shared.Next(-20, 21);
                flameOffsets[c] = (dr, dg, db);
            }

           
            (byte r, byte g, byte b) GetFlameColor(int i,int col) {
                int flameRows = flameArt.Length; // 7
                double t = flameRows > 1 ? (double)i / (flameRows - 1) : 0;
                
                int rBase = 255;
                int gBase = (int)Math.Round(255 * (1 - t) + 69 * t); // 255->69
                int bBase = 0;

               
                var (dr, dg, db) = flameOffsets[col];
                int rr = rBase + dr;
                int gg = gBase + dg;
                int bb = bBase + db;

                
                rr = Math.Max(0,Math.Min(255,rr));
                gg = Math.Max(0,Math.Min(255,gg));
                bb = Math.Max(0,Math.Min(255,bb));

                return ((byte)rr, (byte)gg, (byte)bb);
            }

           
            for (int i = 0; i < flameArt.Length; i++) {
                string line = flameArt[i];
                for (int c = 0; c < line.Length; c++) {
                    char ch = line[c];
                    var (r, g, b) = GetFlameColor(i,c);
                    
                    flameLines[i].Segments.Add(new ColoredText(r,g,b,ch.ToString()));
                }
            }

           
            int textCount = textArt.Length; // 5
            for (int i = 0; i < textCount; i++) {
                int lineIndex = 4 + i; 
                double t = textCount > 1 ? (double)(textCount - 1 - i) / (textCount - 1) : 0;
                int rr = (int)Math.Round(50  * (1 - t) + 173 * t);  // 0->173
                int gg = (int)Math.Round(50  * (1 - t) + 216 * t);  // 0->216
                int bb = (int)Math.Round(139 * (1 - t) + 230 * t);  // 139->230

                textLines[lineIndex].Segments.Add(
                    new ColoredText((byte)rr,(byte)gg,(byte)bb,textArt[i])
                );
            }

            foreach (var line in flameLines.Zip(textLines,MergeLine)) { Console.WriteLine(line);  }

        }
        //-------------------------------------------------------------------------------------------------------------------------------

        //-------------------------------------------------------------------------------------------------------------------------------
        private static ColoredLine MergeLine(ColoredLine flameLine,ColoredLine textLine) {
            if (flameLine == null) flameLine = new ColoredLine();
            if (textLine == null) textLine = new ColoredLine();

            var flameBuf = ExpandLine(flameLine);
            var textBuf  = ExpandLine(textLine);

            int maxLen = Math.Max(flameBuf.Count, textBuf.Count);
            var merged = new ColoredLine();

            // Para acumular runs de color idéntico
            char currentChar = '\0';
            byte cr = 0, cg = 0, cb = 0;
            string accum = "";

            Action flush = () =>             {
                if (accum.Length > 0)                 {
                    merged.Segments.Add(new ColoredText(cr, cg, cb, accum));
                    accum = "";
                }
            };

            for (int i = 0; i < maxLen; i++) {
                (char cf, byte rf, byte gf, byte bf) = i < flameBuf.Count ? flameBuf[i] : (' ', (byte)0, (byte)0, (byte)0);
                (char ct, byte rt, byte gt, byte bt) = i < textBuf.Count ? textBuf[i] : (' ', (byte)0, (byte)0, (byte)0);

                char finalChar;
                byte rr, gg, bb;
                if (cf != ' ') {
                    // Llama con prioridad
                    finalChar = cf;
                    rr = rf; gg = gf; bb = bf;
                }
                else {
                    // Espacio en la llama => usamos texto
                    finalChar = ct;
                    rr = rt; gg = gt; bb = bt;
                }

                // Si cambia de color, flush del run anterior
                if (accum.Length == 0 || rr != cr || gg != cg || bb != cb) {
                    flush();
                    cr = rr; cg = gg; cb = bb;
                    accum = finalChar.ToString();
                }
                else {
                    accum += finalChar;
                }
            }

            flush();
            return merged;
        }
        //-------------------------------------------------------------------------------------------------------------------------------

        //-------------------------------------------------------------------------------------------------------------------------------
        private static List<(char ch, byte r, byte g, byte b)> ExpandLine(ColoredLine line) {
            var result = new List<(char ch, byte r, byte g, byte b)>();
            foreach (var seg in line.Segments) {
                foreach (char c in seg.Text) {
                    result.Add((c, seg.R, seg.G, seg.B));
                }
            }
            return result;
        }
        //-------------------------------------------------------------------------------------------------------------------------------
    }
}
