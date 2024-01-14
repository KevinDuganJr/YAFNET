﻿// Lucene version compatibility level 4.8.1
/*

Copyright (c) 2001, Dr Martin Porter
Copyright (c) 2002, Richard Boulton
All rights reserved.

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions are met:

    * Redistributions of source code must retain the above copyright notice,
    * this list of conditions and the following disclaimer.
    * Redistributions in binary form must reproduce the above copyright
    * notice, this list of conditions and the following disclaimer in the
    * documentation and/or other materials provided with the distribution.
    * Neither the name of the copyright holders nor the names of its contributors
    * may be used to endorse or promote products derived from this software
    * without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE
FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

 */

namespace YAF.Lucene.Net.Tartarus.Snowball.Ext
{
    /// <summary>
    /// This class was automatically generated by a Snowball to Java compiler
    /// It implements the stemming algorithm defined by a snowball script.
    /// </summary>
    public class DutchStemmer : SnowballProgram
    {
        // LUCENENET specific: Factored out methodObject by using Func<bool> instead of Reflection

        private readonly static Among[] a_0 = {
                    new Among ( "", -1, 6 ),
                    new Among ( "\u00E1", 0, 1 ),
                    new Among ( "\u00E4", 0, 1 ),
                    new Among ( "\u00E9", 0, 2 ),
                    new Among ( "\u00EB", 0, 2 ),
                    new Among ( "\u00ED", 0, 3 ),
                    new Among ( "\u00EF", 0, 3 ),
                    new Among ( "\u00F3", 0, 4 ),
                    new Among ( "\u00F6", 0, 4 ),
                    new Among ( "\u00FA", 0, 5 ),
                    new Among ( "\u00FC", 0, 5 )
                };

        private readonly static Among[] a_1 = {
                    new Among ( "", -1, 3 ),
                    new Among ( "I", 0, 2 ),
                    new Among ( "Y", 0, 1 )
                };

        private readonly static Among[] a_2 = {
                    new Among ( "dd", -1, -1 ),
                    new Among ( "kk", -1, -1 ),
                    new Among ( "tt", -1, -1 )
                };

        private readonly static Among[] a_3 = {
                    new Among ( "ene", -1, 2 ),
                    new Among ( "se", -1, 3 ),
                    new Among ( "en", -1, 2 ),
                    new Among ( "heden", 2, 1 ),
                    new Among ( "s", -1, 3 )
                };

        private readonly static Among[] a_4 = {
                    new Among ( "end", -1, 1 ),
                    new Among ( "ig", -1, 2 ),
                    new Among ( "ing", -1, 1 ),
                    new Among ( "lijk", -1, 3 ),
                    new Among ( "baar", -1, 4 ),
                    new Among ( "bar", -1, 5 )
                };

        private readonly static Among[] a_5 = {
                    new Among ( "aa", -1, -1 ),
                    new Among ( "ee", -1, -1 ),
                    new Among ( "oo", -1, -1 ),
                    new Among ( "uu", -1, -1 )
                };

        private readonly static char[] g_v = { (char)17, (char)65, (char)16, (char)1, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)128 };

        private readonly static char[] g_v_I = { (char)1, (char)0, (char)0, (char)17, (char)65, (char)16, (char)1, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)128 };

        private readonly static char[] g_v_j = { (char)17, (char)67, (char)16, (char)1, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)128 };

        private int I_p2;
        private int I_p1;
        private bool B_e_found;

        private void copy_from(DutchStemmer other)
        {
            I_p2 = other.I_p2;
            I_p1 = other.I_p1;
            B_e_found = other.B_e_found;
            base.CopyFrom(other);
        }

        private bool r_prelude()
        {
            int among_var;
            int v_1;
            int v_2;
            int v_3;
            int v_4;
            int v_5;
            int v_6;
            // (, line 41
            // test, line 42
            v_1 = m_cursor;
            // repeat, line 42

            while (true)
            {
                v_2 = m_cursor;

                do
                {
                    // (, line 42
                    // [, line 43
                    m_bra = m_cursor;
                    // substring, line 43
                    among_var = FindAmong(a_0, 11);
                    if (among_var == 0)
                    {
                        goto lab1;
                    }
                    // ], line 43
                    m_ket = m_cursor;
                    switch (among_var)
                    {
                        case 0:
                            goto lab1;
                        case 1:
                            // (, line 45
                            // <-, line 45
                            SliceFrom("a");
                            break;
                        case 2:
                            // (, line 47
                            // <-, line 47
                            SliceFrom("e");
                            break;
                        case 3:
                            // (, line 49
                            // <-, line 49
                            SliceFrom("i");
                            break;
                        case 4:
                            // (, line 51
                            // <-, line 51
                            SliceFrom("o");
                            break;
                        case 5:
                            // (, line 53
                            // <-, line 53
                            SliceFrom("u");
                            break;
                        case 6:
                            // (, line 54
                            // next, line 54
                            if (m_cursor >= m_limit)
                            {
                                goto lab1;
                            }
                            m_cursor++;
                            break;
                    }
                    // LUCENENET NOTE: continue label is not supported directly in .NET,
                    // so we just need to add another goto to get to the end of the outer loop.
                    // See: http://stackoverflow.com/a/359449/181087

                    // Original code:
                    //continue replab0;

                    goto end_of_outer_loop;

                } while (false);
                lab1:
                m_cursor = v_2;
                goto replab0;
                end_of_outer_loop: { }
            }
            replab0:
            m_cursor = v_1;
            // try, line 57
            v_3 = m_cursor;

            do
            {
                // (, line 57
                // [, line 57
                m_bra = m_cursor;
                // literal, line 57
                if (!(Eq_S(1, "y")))
                {
                    m_cursor = v_3;
                    goto lab2;
                }
                // ], line 57
                m_ket = m_cursor;
                // <-, line 57
                SliceFrom("Y");
            } while (false);
            lab2:
            // repeat, line 58

            while (true)
            {
                v_4 = m_cursor;

                do
                {
                    // goto, line 58

                    while (true)
                    {
                        v_5 = m_cursor;

                        do
                        {
                            // (, line 58
                            if (!(InGrouping(g_v, 97, 232)))
                            {
                                goto lab6;
                            }
                            // [, line 59
                            m_bra = m_cursor;
                            // or, line 59

                            do
                            {
                                v_6 = m_cursor;

                                do
                                {
                                    // (, line 59
                                    // literal, line 59
                                    if (!(Eq_S(1, "i")))
                                    {
                                        goto lab8;
                                    }
                                    // ], line 59
                                    m_ket = m_cursor;
                                    if (!(InGrouping(g_v, 97, 232)))
                                    {
                                        goto lab8;
                                    }
                                    // <-, line 59
                                    SliceFrom("I");
                                    goto lab7;
                                } while (false);
                                lab8:
                                m_cursor = v_6;
                                // (, line 60
                                // literal, line 60
                                if (!(Eq_S(1, "y")))
                                {
                                    goto lab6;
                                }
                                // ], line 60
                                m_ket = m_cursor;
                                // <-, line 60
                                SliceFrom("Y");
                            } while (false);
                            lab7:
                            m_cursor = v_5;
                            goto golab5;
                        } while (false);
                        lab6:
                        m_cursor = v_5;
                        if (m_cursor >= m_limit)
                        {
                            goto lab4;
                        }
                        m_cursor++;
                    }
                    golab5:
                    // LUCENENET NOTE: continue label is not supported directly in .NET,
                    // so we just need to add another goto to get to the end of the outer loop.
                    // See: http://stackoverflow.com/a/359449/181087

                    // Original code:
                    //continue replab3;

                    goto end_of_outer_loop;

                } while (false);
                lab4:
                m_cursor = v_4;
                goto replab3;
                end_of_outer_loop: { }
            }
            replab3:
            return true;
        }

        private bool r_mark_regions()
        {
            // (, line 64
            I_p1 = m_limit;
            I_p2 = m_limit;
            // gopast, line 69

            while (true)
            {

                do
                {
                    if (!(InGrouping(g_v, 97, 232)))
                    {
                        goto lab1;
                    }
                    goto golab0;
                } while (false);
                lab1:
                if (m_cursor >= m_limit)
                {
                    return false;
                }
                m_cursor++;
            }
            golab0:
            // gopast, line 69

            while (true)
            {

                do
                {
                    if (!(OutGrouping(g_v, 97, 232)))
                    {
                        goto lab3;
                    }
                    goto golab2;
                } while (false);
                lab3:
                if (m_cursor >= m_limit)
                {
                    return false;
                }
                m_cursor++;
            }
            golab2:
            // setmark p1, line 69
            I_p1 = m_cursor;
            // try, line 70

            do
            {
                // (, line 70
                if (!(I_p1 < 3))
                {
                    goto lab4;
                }
                I_p1 = 3;
            } while (false);
            lab4:
            // gopast, line 71

            while (true)
            {

                do
                {
                    if (!(InGrouping(g_v, 97, 232)))
                    {
                        goto lab6;
                    }
                    goto golab5;
                } while (false);
                lab6:
                if (m_cursor >= m_limit)
                {
                    return false;
                }
                m_cursor++;
            }
            golab5:
            // gopast, line 71

            while (true)
            {

                do
                {
                    if (!(OutGrouping(g_v, 97, 232)))
                    {
                        goto lab8;
                    }
                    goto golab7;
                } while (false);
                lab8:
                if (m_cursor >= m_limit)
                {
                    return false;
                }
                m_cursor++;
            }
            golab7:
            // setmark p2, line 71
            I_p2 = m_cursor;
            return true;
        }

        private bool r_postlude()
        {
            int among_var;
            int v_1;
            // repeat, line 75

            while (true)
            {
                v_1 = m_cursor;

                do
                {
                    // (, line 75
                    // [, line 77
                    m_bra = m_cursor;
                    // substring, line 77
                    among_var = FindAmong(a_1, 3);
                    if (among_var == 0)
                    {
                        goto lab1;
                    }
                    // ], line 77
                    m_ket = m_cursor;
                    switch (among_var)
                    {
                        case 0:
                            goto lab1;
                        case 1:
                            // (, line 78
                            // <-, line 78
                            SliceFrom("y");
                            break;
                        case 2:
                            // (, line 79
                            // <-, line 79
                            SliceFrom("i");
                            break;
                        case 3:
                            // (, line 80
                            // next, line 80
                            if (m_cursor >= m_limit)
                            {
                                goto lab1;
                            }
                            m_cursor++;
                            break;
                    }
                    // LUCENENET NOTE: continue label is not supported directly in .NET,
                    // so we just need to add another goto to get to the end of the outer loop.
                    // See: http://stackoverflow.com/a/359449/181087

                    // Original code:
                    //continue replab0;

                    goto end_of_outer_loop;

                } while (false);
                lab1:
                m_cursor = v_1;
                goto replab0;
                end_of_outer_loop: { }
            }
            replab0:
            return true;
        }

        private bool r_R1()
        {
            if (!(I_p1 <= m_cursor))
            {
                return false;
            }
            return true;
        }

        private bool r_R2()
        {
            if (!(I_p2 <= m_cursor))
            {
                return false;
            }
            return true;
        }

        private bool r_undouble()
        {
            int v_1;
            // (, line 90
            // test, line 91
            v_1 = m_limit - m_cursor;
            // among, line 91
            if (FindAmongB(a_2, 3) == 0)
            {
                return false;
            }
            m_cursor = m_limit - v_1;
            // [, line 91
            m_ket = m_cursor;
            // next, line 91
            if (m_cursor <= m_limit_backward)
            {
                return false;
            }
            m_cursor--;
            // ], line 91
            m_bra = m_cursor;
            // delete, line 91
            SliceDel();
            return true;
        }

        private bool r_e_ending()
        {
            int v_1;
            // (, line 94
            // unset e_found, line 95
            B_e_found = false;
            // [, line 96
            m_ket = m_cursor;
            // literal, line 96
            if (!(Eq_S_B(1, "e")))
            {
                return false;
            }
            // ], line 96
            m_bra = m_cursor;
            // call R1, line 96
            if (!r_R1())
            {
                return false;
            }
            // test, line 96
            v_1 = m_limit - m_cursor;
            if (!(OutGroupingB(g_v, 97, 232)))
            {
                return false;
            }
            m_cursor = m_limit - v_1;
            // delete, line 96
            SliceDel();
            // set e_found, line 97
            B_e_found = true;
            // call undouble, line 98
            if (!r_undouble())
            {
                return false;
            }
            return true;
        }

        private bool r_en_ending()
        {
            int v_1;
            int v_2;
            // (, line 101
            // call R1, line 102
            if (!r_R1())
            {
                return false;
            }
            // and, line 102
            v_1 = m_limit - m_cursor;
            if (!(OutGroupingB(g_v, 97, 232)))
            {
                return false;
            }
            m_cursor = m_limit - v_1;
            // not, line 102
            {
                v_2 = m_limit - m_cursor;

                do
                {
                    // literal, line 102
                    if (!(Eq_S_B(3, "gem")))
                    {
                        goto lab0;
                    }
                    return false;
                } while (false);
                lab0:
                m_cursor = m_limit - v_2;
            }
            // delete, line 102
            SliceDel();
            // call undouble, line 103
            if (!r_undouble())
            {
                return false;
            }
            return true;
        }

        private bool r_standard_suffix()
        {
            int among_var;
            int v_1;
            int v_2;
            int v_3;
            int v_4;
            int v_5;
            int v_6;
            int v_7;
            int v_8;
            int v_9;
            int v_10;
            // (, line 106
            // do, line 107
            v_1 = m_limit - m_cursor;

            do
            {
                // (, line 107
                // [, line 108
                m_ket = m_cursor;
                // substring, line 108
                among_var = FindAmongB(a_3, 5);
                if (among_var == 0)
                {
                    goto lab0;
                }
                // ], line 108
                m_bra = m_cursor;
                switch (among_var)
                {
                    case 0:
                        goto lab0;
                    case 1:
                        // (, line 110
                        // call R1, line 110
                        if (!r_R1())
                        {
                            goto lab0;
                        }
                        // <-, line 110
                        SliceFrom("heid");
                        break;
                    case 2:
                        // (, line 113
                        // call en_ending, line 113
                        if (!r_en_ending())
                        {
                            goto lab0;
                        }
                        break;
                    case 3:
                        // (, line 116
                        // call R1, line 116
                        if (!r_R1())
                        {
                            goto lab0;
                        }
                        if (!(OutGroupingB(g_v_j, 97, 232)))
                        {
                            goto lab0;
                        }
                        // delete, line 116
                        SliceDel();
                        break;
                }
            } while (false);
            lab0:
            m_cursor = m_limit - v_1;
            // do, line 120
            v_2 = m_limit - m_cursor;

            do
            {
                // call e_ending, line 120
                if (!r_e_ending())
                {
                    goto lab1;
                }
            } while (false);
            lab1:
            m_cursor = m_limit - v_2;
            // do, line 122
            v_3 = m_limit - m_cursor;

            do
            {
                // (, line 122
                // [, line 122
                m_ket = m_cursor;
                // literal, line 122
                if (!(Eq_S_B(4, "heid")))
                {
                    goto lab2;
                }
                // ], line 122
                m_bra = m_cursor;
                // call R2, line 122
                if (!r_R2())
                {
                    goto lab2;
                }
                // not, line 122
                {
                    v_4 = m_limit - m_cursor;

                    do
                    {
                        // literal, line 122
                        if (!(Eq_S_B(1, "c")))
                        {
                            goto lab3;
                        }
                        goto lab2;
                    } while (false);
                    lab3:
                    m_cursor = m_limit - v_4;
                }
                // delete, line 122
                SliceDel();
                // [, line 123
                m_ket = m_cursor;
                // literal, line 123
                if (!(Eq_S_B(2, "en")))
                {
                    goto lab2;
                }
                // ], line 123
                m_bra = m_cursor;
                // call en_ending, line 123
                if (!r_en_ending())
                {
                    goto lab2;
                }
            } while (false);
            lab2:
            m_cursor = m_limit - v_3;
            // do, line 126
            v_5 = m_limit - m_cursor;

            do
            {
                // (, line 126
                // [, line 127
                m_ket = m_cursor;
                // substring, line 127
                among_var = FindAmongB(a_4, 6);
                if (among_var == 0)
                {
                    goto lab4;
                }
                // ], line 127
                m_bra = m_cursor;
                switch (among_var)
                {
                    case 0:
                        goto lab4;
                    case 1:
                        // (, line 129
                        // call R2, line 129
                        if (!r_R2())
                        {
                            goto lab4;
                        }
                        // delete, line 129
                        SliceDel();
                        // or, line 130

                        do
                        {
                            v_6 = m_limit - m_cursor;

                            do
                            {
                                // (, line 130
                                // [, line 130
                                m_ket = m_cursor;
                                // literal, line 130
                                if (!(Eq_S_B(2, "ig")))
                                {
                                    goto lab6;
                                }
                                // ], line 130
                                m_bra = m_cursor;
                                // call R2, line 130
                                if (!r_R2())
                                {
                                    goto lab6;
                                }
                                // not, line 130
                                {
                                    v_7 = m_limit - m_cursor;

                                    do
                                    {
                                        // literal, line 130
                                        if (!(Eq_S_B(1, "e")))
                                        {
                                            goto lab7;
                                        }
                                        goto lab6;
                                    } while (false);
                                    lab7:
                                    m_cursor = m_limit - v_7;
                                }
                                // delete, line 130
                                SliceDel();
                                goto lab5;
                            } while (false);
                            lab6:
                            m_cursor = m_limit - v_6;
                            // call undouble, line 130
                            if (!r_undouble())
                            {
                                goto lab4;
                            }
                        } while (false);
                        lab5:
                        break;
                    case 2:
                        // (, line 133
                        // call R2, line 133
                        if (!r_R2())
                        {
                            goto lab4;
                        }
                        // not, line 133
                        {
                            v_8 = m_limit - m_cursor;

                            do
                            {
                                // literal, line 133
                                if (!(Eq_S_B(1, "e")))
                                {
                                    goto lab8;
                                }
                                goto lab4;
                            } while (false);
                            lab8:
                            m_cursor = m_limit - v_8;
                        }
                        // delete, line 133
                        SliceDel();
                        break;
                    case 3:
                        // (, line 136
                        // call R2, line 136
                        if (!r_R2())
                        {
                            goto lab4;
                        }
                        // delete, line 136
                        SliceDel();
                        // call e_ending, line 136
                        if (!r_e_ending())
                        {
                            goto lab4;
                        }
                        break;
                    case 4:
                        // (, line 139
                        // call R2, line 139
                        if (!r_R2())
                        {
                            goto lab4;
                        }
                        // delete, line 139
                        SliceDel();
                        break;
                    case 5:
                        // (, line 142
                        // call R2, line 142
                        if (!r_R2())
                        {
                            goto lab4;
                        }
                        // Boolean test e_found, line 142
                        if (!(B_e_found))
                        {
                            goto lab4;
                        }
                        // delete, line 142
                        SliceDel();
                        break;
                }
            } while (false);
            lab4:
            m_cursor = m_limit - v_5;
            // do, line 146
            v_9 = m_limit - m_cursor;

            do
            {
                // (, line 146
                if (!(OutGroupingB(g_v_I, 73, 232)))
                {
                    goto lab9;
                }
                // test, line 148
                v_10 = m_limit - m_cursor;
                // (, line 148
                // among, line 149
                if (FindAmongB(a_5, 4) == 0)
                {
                    goto lab9;
                }
                if (!(OutGroupingB(g_v, 97, 232)))
                {
                    goto lab9;
                }
                m_cursor = m_limit - v_10;
                // [, line 152
                m_ket = m_cursor;
                // next, line 152
                if (m_cursor <= m_limit_backward)
                {
                    goto lab9;
                }
                m_cursor--;
                // ], line 152
                m_bra = m_cursor;
                // delete, line 152
                SliceDel();
            } while (false);
            lab9:
            m_cursor = m_limit - v_9;
            return true;
        }


        public override bool Stem()
        {
            int v_1;
            int v_2;
            int v_3;
            int v_4;
            // (, line 157
            // do, line 159
            v_1 = m_cursor;

            do
            {
                // call prelude, line 159
                if (!r_prelude())
                {
                    goto lab0;
                }
            } while (false);
            lab0:
            m_cursor = v_1;
            // do, line 160
            v_2 = m_cursor;

            do
            {
                // call mark_regions, line 160
                if (!r_mark_regions())
                {
                    goto lab1;
                }
            } while (false);
            lab1:
            m_cursor = v_2;
            // backwards, line 161
            m_limit_backward = m_cursor; m_cursor = m_limit;
            // do, line 162
            v_3 = m_limit - m_cursor;

            do
            {
                // call standard_suffix, line 162
                if (!r_standard_suffix())
                {
                    goto lab2;
                }
            } while (false);
            lab2:
            m_cursor = m_limit - v_3;
            m_cursor = m_limit_backward;                    // do, line 163
            v_4 = m_cursor;

            do
            {
                // call postlude, line 163
                if (!r_postlude())
                {
                    goto lab3;
                }
            } while (false);
            lab3:
            m_cursor = v_4;
            return true;
        }


        public override bool Equals(object o)
        {
            return o is DutchStemmer;
        }

        public override int GetHashCode()
        {
            return this.GetType().FullName.GetHashCode();
        }
    }
}