﻿using System;
using System.Text;

namespace NCurses.Core.Interop
{
    public interface INCursesWrapper
    {
        int COLOR_PAIR(int pair);
        int PAIR_NUMBER(uint attrs);
        IntPtr _nc_screen_of(IntPtr window);
        bool _nc_unicode_locale();
        int add_wch(IntPtr wch);
        int add_wchnstr(IntPtr wchStr, int n);
        int add_wchstr(IntPtr wchStr);
        int addch(uint ch);
        int addchnstr(IntPtr txt, int number);
        int addchstr(IntPtr txt);
        int addnstr(string txt, int number);
        int addnwstr(IntPtr wstr, int n);
        int addstr(string txt);
        int addwstr(string wstr);
        int assume_default_colors(int fg, int bg);
        int assume_default_colors_sp(IntPtr screen, int fg, int bg);
        int attr_get(IntPtr attrs, IntPtr pair, IntPtr opts);
        int attr_off(uint attrs, IntPtr opts);
        int attr_on(uint attrs, IntPtr opts);
        int attr_set(uint attrs, short pair, IntPtr opts);
        int attroff(int attrs);
        int attron(int attrs);
        int attrset(int attrs);
        int baudrate();
        int baudrate_sp(IntPtr screen);
        int beep();
        int beep_sp(IntPtr screen);
        int bkgd(uint bkgd);
        void bkgdset(uint bkgd);
        int bkgrnd(IntPtr wch);
        int bkgrnd(IntPtr window, IntPtr wch);
        void bkgrndset(IntPtr wch);
        int border(uint ls, uint rs, uint ts, uint bs, uint tl, uint tr, uint bl, uint br);
        int border_set(IntPtr ls, IntPtr rs, IntPtr ts, IntPtr bs, IntPtr tl, IntPtr tr, IntPtr bl, IntPtr br);
        int box(IntPtr window, uint verch, uint horch);
        int box_set(IntPtr window, IntPtr verch, IntPtr horch);
        bool can_change_color();
        bool can_change_color_sp(IntPtr screen);
        int cbreak();
        int cbreak_sp(IntPtr screen);
        int chgat(int number, uint attrs, short pair, IntPtr opts);
        int clear();
        int clearok(IntPtr window, bool bf);
        int clrtobot();
        int clrtoeol();
        int color_content(short color, IntPtr red, IntPtr green, IntPtr blue);
        int color_content_sp(IntPtr screen, short color, IntPtr red, IntPtr green, IntPtr blue);
        int color_set(short pair, IntPtr opts);
        int copywin(IntPtr srcwin, IntPtr dstwin, int sminrow, int smincol, int dminrow, int dmincol, int dmaxrow, int dmaxcol, int overlay);
        int curs_set(int visibility);
        int curs_set_sp(IntPtr screen, int visibility);
        string curses_version();
        int def_prog_mode();
        int def_prog_mode_sp(IntPtr screen);
        int def_shell_mode();
        int def_shell_mode_sp(IntPtr screen);
        int define_key(string definition, int keycode);
        int define_key_sp(IntPtr screen, string definition, int keycode);
        int delay_output(int ms);
        int delay_output_sp(IntPtr screen, int ms);
        int delch();
        int deleteln();
        void delscreen(IntPtr screen);
        int delwin(IntPtr window);
        IntPtr derwin(IntPtr window, int nlines, int ncols, int begin_y, int begin_x);
        int doupdate();
        int doupdate_sp(IntPtr screen);
        IntPtr dupwin(IntPtr window);
        int echo();
        int echo_sp(IntPtr screen);
        int echo_wchar(IntPtr wch);
        int echochar(uint ch);
        int endwin();
        int endwin_sp(IntPtr screen);
        int erase();
        char erasechar();
        char erasechar_sp(IntPtr screen);
        int erasewcharr(IntPtr wch);
        void filter();
        void filter_sp(IntPtr screen);
        int flash();
        int flash_sp(IntPtr screen);
        int flushinp();
        int flushinp_sp(IntPtr screen);
        int get_escdelay();
        int get_escdelay_sp(IntPtr screen);
        int get_wch(IntPtr wch);
        int get_wstr(IntPtr wstr);
        int getattrs(IntPtr window);
        int getbegx(IntPtr window);
        int getbegy(IntPtr window);
        uint getbkgd(IntPtr window);
        int getbkgrnd(IntPtr wch);
        int getcchar(IntPtr wcval, IntPtr wch, out uint attrs, out short color_pair, IntPtr opts);
        int getch();
        int getcurx(IntPtr window);
        int getcury(IntPtr window);
        int getmaxx(IntPtr window);
        int getmaxy(IntPtr window);
        int getmouse(out MEVENT ev);
        int getmouse_sp(IntPtr screen, IntPtr ev);
        int getmouse_sp(IntPtr screen, ref MEVENT ev);
        int getn_wstr(IntPtr wstr, int n);
        int getnstr(StringBuilder builder, int count);
        int getparx(IntPtr window);
        int getpary(IntPtr window);
        int getstr(StringBuilder builder);
        int halfdelay(int tenths);
        int halfdelay_sp(IntPtr screen, int tenths);
        bool has_colors();
        bool has_colors_sp(IntPtr screen);
        bool has_ic();
        bool has_ic_sp(IntPtr screen);
        bool has_il();
        bool has_il_sp(IntPtr screen);
        int has_key(int ch);
        bool has_mouse();
        bool has_mouse_sp(IntPtr screen);
        int hline(uint ch, int count);
        int hline_set(IntPtr wch, int n);
        void idcok(IntPtr window, bool bf);
        int idlok(IntPtr window, bool bf);
        void immedok(IntPtr window, bool bf);
        int in_wch(IntPtr wch);
        int in_wchnstr(IntPtr wch, int n);
        int in_wchstrr(IntPtr wch);
        uint inch();
        int inchnstr(IntPtr txt, int count);
        int inchstr(IntPtr txt);
        int init_color(short color, short r, short g, short b);
        int init_color_sp(IntPtr screen, short color, short r, short g, short b);
        int init_pair(short pair, short f, short b);
        int init_pair_sp(IntPtr screen, short pair, short f, short b);
        IntPtr initscr();
        int innstr(StringBuilder str, int n);
        int innwstr(IntPtr str, int n);
        int ins_nwstr(IntPtr str, int n);
        int ins_wch(IntPtr wch);
        int ins_wstr(IntPtr str);
        int insch(uint ch);
        int insdelln(int n);
        int insertln();
        int insnstr(string str, int n);
        int insstr(string str);
        int instr(StringBuilder str);
        int intrflush(IntPtr win, bool bf);
        int intrflush_sp(IntPtr screen, IntPtr win, bool bf);
        int inwstr(IntPtr str);
        bool is_cleared(IntPtr window);
        bool is_idcok(IntPtr window);
        bool is_idlok(IntPtr window);
        bool is_immedok(IntPtr window);
        bool is_keypad(IntPtr window);
        bool is_leaveok(IntPtr window);
        bool is_linetouched(IntPtr window, int line);
        bool is_nodelay(IntPtr window);
        bool is_notimeout(IntPtr window);
        bool is_pad(IntPtr window);
        bool is_scrollok(IntPtr window);
        bool is_subwin(IntPtr window);
        bool is_syncok(IntPtr window);
        bool is_term_resized(int lines, int columns);
        bool is_term_resized_sp(IntPtr screen, int lines, int columns);
        bool is_wintouched(IntPtr window);
        bool isendwin();
        bool isendwin_sp(IntPtr screen);
        int key_defined(string definition);
        int key_defined_sp(IntPtr screen, string definition);
        IntPtr key_name(char c);
        string keybound(int keycode, int count);
        string keybound_sp(IntPtr screen, int keycode, int count);
        IntPtr keyname(int c);
        string keyname_sp(IntPtr screen, int c);
        int keyok(int keycode, bool enable);
        int keyok_sp(IntPtr screen, int keycode, bool enable);
        int keypad(IntPtr window, bool bf);
        char killchar();
        char killchar_sp(IntPtr screen);
        int killwchar(ref char wch);
        int leaveok(IntPtr window, bool bf);
        string longname();
        string longname_sp(IntPtr screen);
        int meta(IntPtr win, bool bf);
        bool mouse_trafo(ref int pY, ref int pX, bool to_screen);
        int mouseinterval(int erval);
        int mouseinterval_sp(IntPtr screen, int erval);
        uint mousemask(uint newmask, out uint oldmask);
        uint mousemask_sp(IntPtr screen, uint newmask, ref uint? oldmask);
        int move(int y, int x);
        int mvadd_wch(int y, int x, IntPtr wch);
        int mvadd_wchnstr(int y, int x, IntPtr wch, int n);
        int mvadd_wchstr(int y, int x, IntPtr wch);
        int mvaddch(int y, int x, uint ch);
        int mvaddchnstr(int y, int x, IntPtr chstr, int n);
        int mvaddchstr(int y, int x, IntPtr chstr);
        int mvaddnstr(int y, int x, string txt, int n);
        int mvaddnwstr(int y, int x, IntPtr wstr, int n);
        int mvaddstr(int y, int x, string txt);
        int mvaddwstr(int y, int x, IntPtr wstr);
        int mvchgat(int y, int x, int number, uint attrs, short pair);
        int mvcur(int oldrow, int oldcol, int newrow, int newcol);
        int mvcur_sp(IntPtr screen, int oldrow, int oldcol, int newrow, int newcol);
        int mvdelch(int y, int x);
        int mvderwin(IntPtr window, int par_y, int par_x);
        int mvget_wch(int y, int x, IntPtr wch);
        int mvget_wstr(int y, int x, IntPtr wstr);
        int mvgetch(int y, int x);
        int mvgetn_wstr(int y, int x, IntPtr wstr, int n);
        int mvgetnstr(int y, int x, StringBuilder str, int count);
        int mvgetstr(int y, int x, StringBuilder str);
        int mvhline(int y, int x, uint ch, int count);
        int mvhline_set(int y, int x, IntPtr wch, int n);
        int mvin_wch(int y, int x, IntPtr wch);
        int mvin_wchnstr(int y, int x, IntPtr wch, int n);
        uint mvinch(int y, int x);
        int mvinchnstr(int y, int x, IntPtr txt, int count);
        int mvinchstr(int y, int x, IntPtr txt);
        int mvinnstr(int y, int x, StringBuilder str, int n);
        int mvinnwstr(int y, int x, IntPtr str, int n);
        int mvins_nwstr(int y, int x, IntPtr str, int n);
        int mvins_wch(int y, int x, IntPtr wch);
        int mvins_wstr(int y, int x, IntPtr str);
        int mvinsch(int y, int x, uint ch);
        int mvinsnstr(int y, int x, string str, int n);
        int mvinsstr(int y, int x, string str);
        int mvinstr(int y, int x, StringBuilder str);
        int mvinwstr(int y, int x, IntPtr str);
        int mvprintw(int y, int x, string format, object[] var);
        int mvscanw(int y, int x, StringBuilder format, object[] var);
        int mvvline(int y, int x, uint ch, int n);
        int mvvline_set(int y, int x, IntPtr wch, int n);
        int mvwadd_wch(IntPtr window, int y, int x, IntPtr wch);
        int mvwadd_wchnstr(IntPtr window, int y, int x, IntPtr wch, int n);
        int mvwadd_wchstr(IntPtr window, int y, int x, IntPtr wchStr);
        int mvwaddch(IntPtr window, int y, int x, uint ch);
        int mvwaddchnstr(IntPtr window, int y, int x, IntPtr chstr, int n);
        int mvwaddchstr(IntPtr window, int y, int x, IntPtr chstr);
        int mvwaddnstr(IntPtr window, int y, int x, string txt, int n);
        int mvwaddnwstr(IntPtr window, int y, int x, IntPtr wstr, int n);
        int mvwaddstr(IntPtr window, int y, int x, string txt);
        int mvwaddwstr(IntPtr window, int y, int x, IntPtr wstr);
        int mvwchgat(IntPtr window, int y, int x, int number, uint attrs, short pair);
        int mvwdelch(IntPtr window, int y, int x);
        int mvwget_wch(IntPtr window, int y, int x, IntPtr wch);
        int mvwget_wstr(IntPtr window, int y, int x, IntPtr wstr);
        int mvwgetch(IntPtr window, int y, int x);
        int mvwgetn_wstr(IntPtr window, int y, int x, IntPtr wstr, int n);
        int mvwgetnstr(IntPtr window, int y, int x, StringBuilder str, int count);
        int mvwgetstr(IntPtr window, int y, int x, StringBuilder str);
        int mvwhline(IntPtr window, int y, int x, uint ch, int count);
        int mvwhline_set(IntPtr window, int y, int x, IntPtr wch, int n);
        int mvwin(IntPtr win, int y, int x);
        int mvwin_wch(IntPtr window, int y, int x, IntPtr wch);
        int mvwin_wchnstr(IntPtr window, int y, int x, IntPtr wch, int n);
        uint mvwinch(IntPtr window, int y, int x);
        int mvwinchnstr(IntPtr window, int y, int x, IntPtr txt, int count);
        int mvwinchstr(IntPtr window, int y, int x, IntPtr txt);
        int mvwinnstr(IntPtr window, int y, int x, StringBuilder str, int n);
        int mvwinnwstr(IntPtr window, int y, int x, IntPtr str, int n);
        int mvwins_nwstr(IntPtr window, int y, int x, IntPtr str, int n);
        int mvwins_wch(IntPtr window, int y, int x, IntPtr wch);
        int mvwins_wstr(IntPtr window, int y, int x, IntPtr str);
        int mvwinsch(IntPtr window, int y, int x, uint ch);
        int mvwinsnstr(IntPtr window, int y, int x, string str, int n);
        int mvwinsstr(IntPtr window, int y, int x, string str);
        int mvwinstr(IntPtr window, int y, int x, StringBuilder str);
        int mvwinwstr(IntPtr window, int y, int x, IntPtr str);
        int mvwprintw(IntPtr window, int y, int x, string format, object[] var);
        int mvwscanw(IntPtr window, int y, int x, StringBuilder format, object[] var);
        int mvwvline(IntPtr window, int y, int x, uint ch, int n);
        int mvwvline_set(IntPtr window, int y, int x, IntPtr wch, int n);
        int napms(int ms);
        int napms_sp(IntPtr screen, int ms);
        IntPtr new_prescr();
        IntPtr newpad(int nlines, int ncols);
        IntPtr newpad_sp(IntPtr screen, int nlines, int ncols);
        IntPtr newwin(int nlines, int ncols, int begin_y, int begin_x);
        IntPtr newwin_sp(IntPtr screen, int nlines, int ncols, int begin_y, int begin_x);
        int nl();
        int nl_sp(IntPtr screen);
        int nocbreak();
        int nocbreak_sp(IntPtr screen);
        int nodelay(IntPtr window, bool bf);
        int noecho();
        int noecho_sp(IntPtr screen);
        void nofilter();
        void nofilter_sp(IntPtr screen);
        int nonl();
        int nonl_sp(IntPtr screen);
        void noqiflush();
        void noqiflush_sp(IntPtr screen);
        int noraw();
        int noraw_sp(IntPtr screen);
        int notimeout(IntPtr window, bool bf);
        int overlay(IntPtr srcWin, IntPtr destWin);
        int overwrite(IntPtr srcWin, IntPtr destWin);
        int pair_content(short pair, out short fg, out short bg);
        int pair_content_sp(IntPtr screen, short pair, IntPtr f, IntPtr b);
        int pecho_wchar(IntPtr pad, IntPtr wch);
        int pechochar(IntPtr pad, uint ch);
        int pnoutrefresh(IntPtr pad, int pminrow, int pmincol, int sminrow, int smincol, int smaxrow, int smaxcol);
        int prefresh(IntPtr pad, int pminrow, int pmincol, int sminrow, int smincol, int smaxrow, int smaxcol);
        int printw(string format, object[] var);
        int putp(string str);
        int putp_sp(IntPtr screen, string str);
        void qiflush();
        void qiflush_sp(IntPtr screen);
        int raw();
        int raw_sp(IntPtr screen);
        int redrawwin();
        int refresh();
        int reset_prog_mode();
        int reset_prog_mode_sp(IntPtr screen);
        int reset_shell_mode();
        int reset_shell_mode_sp(IntPtr screen);
        int resetty();
        int resetty_sp(IntPtr screen);
        int resize_term(int lines, int columns);
        int resize_term_sp(IntPtr screen, int lines, int columns);
        int resizeterm(int lines, int columns);
        int resizeterm_sp(IntPtr screen, int lines, int columns);
        int ripoffline(int line, IntPtr method);
        int ripoffline_sp(IntPtr screen, int line, IntPtr method);
        int savetty();
        int savetty_sp(IntPtr screen);
        int scanw(StringBuilder format, object[] var);
        int scr_dump(string filename);
        int scr_dump_sp(IntPtr screen, string filename);
        int scr_init(string filename);
        int scr_init_sp(IntPtr screen, string filename);
        int scr_restore(string filename);
        int scr_restore_sp(IntPtr screen, string filename);
        int scr_set(string filename);
        int scr_set_sp(IntPtr screen, string filename);
        int scrl(int n);
        int scroll(IntPtr window);
        int scrollok(IntPtr window, bool bf);
        int set_escdelay(int size);
        int set_escdelay_sp(IntPtr screen, int size);
        int set_tabsize(int size);
        int set_tabsize_sp(IntPtr screen, int size);
        IntPtr set_term(IntPtr newScr);
        int setcchar(IntPtr wcval, IntPtr wch, uint attrs, short color_pair, IntPtr opts);
        int setscrreg(int top, int bot);
        uint slk_attr();
        int slk_attr_off(uint attrs, IntPtr opts);
        int slk_attr_on(uint attrs, IntPtr opts);
        int slk_attr_set(uint attrs, short color_pair, IntPtr opts);
        int slk_attr_set_sp(IntPtr screen, uint attrs, short color_pair, IntPtr opts);
        uint slk_attr_sp(IntPtr screen);
        int slk_attroff(uint attrs);
        int slk_attroff_sp(IntPtr screen, uint attrs);
        int slk_attron(uint attrs);
        int slk_attron_sp(IntPtr screen, uint attrs);
        int slk_attrset(uint attrs, short color_pair, IntPtr opts);
        int slk_attrset_sp(IntPtr screen, uint attrs, short color_pair, IntPtr opts);
        int slk_clear();
        int slk_clear_sp(IntPtr screen);
        int slk_color(short color_pair);
        int slk_color_sp(IntPtr screen, short color_pair);
        int slk_init(int fmt);
        int slk_init_sp(IntPtr screen, int fmt);
        string slk_label(int labnum);
        string slk_label_sp(IntPtr screen, int labnum);
        int slk_noutrefresh();
        int slk_noutrefresh_sp(IntPtr screen);
        int slk_refresh();
        int slk_refresh_sp(IntPtr screen);
        int slk_restore();
        int slk_restore_sp(IntPtr screen);
        int slk_set(int labnum, string label, int fmt);
        int slk_set_sp(IntPtr screen, int labnum, string label, int fmt);
        int slk_touch();
        int slk_touch_sp(IntPtr screen);
        int slk_wset(int labnum, string label, int fmt);
        int standend();
        int standout();
        int start_color();
        int start_color_sp(IntPtr screen);
        IntPtr subpad(IntPtr orig, int nlines, int ncols, int begin_y, int begin_x);
        IntPtr subwin(IntPtr orig, int nlines, int ncols, int begin_y, int begin_x);
        int syncok(IntPtr window, bool bf);
        uint term_attrs();
        uint term_attrs_sp(IntPtr screen);
        uint termattrs();
        uint termattrs_sp(IntPtr screen);
        string termname();
        string termname_sp(IntPtr screen);
        int tigetflag(string capname);
        int tigetflag_sp(IntPtr screen, string capname);
        int tigetnum(string capname);
        int tigetnum_sp(IntPtr screen, string capname);
        string tigetstr(string capname);
        string tigetstr_sp(IntPtr screen, string capname);
        void timeout(int delay);
        int touchline(IntPtr window, int start, int count);
        int touchwin(IntPtr window);
        int typeahead(int fd);
        int typeahead_sp(IntPtr screen, int fd);
        string unctrl(uint ch);
        int unget_wch(char wch);
        int unget_wch_sp(IntPtr screen, char wch);
        int ungetch(int ch);
        int ungetch_sp(IntPtr screen, int ch);
        int ungetmouse(MEVENT ev);
        int ungetmouse_sp(IntPtr screen, MEVENT ev);
        int untouchwin(IntPtr window);
        int use_default_colors();
        int use_default_colors_sp(IntPtr screen);
        void use_env(bool f);
        void use_env_sp(IntPtr screen, bool f);
        int use_extended_names(bool enable);
        int use_legacy_coding(int level);
        int use_legacy_coding_sp(IntPtr screen, int level);
        int use_screen(IntPtr screen, IntPtr callback, IntPtr args);
        void use_tioctl(bool f);
        void use_tioctl_sp(IntPtr screen, bool f);
        int use_window(IntPtr window, IntPtr callback, IntPtr args);
        int vid_attr(uint attrs, short pair, IntPtr opts);
        int vid_attr_sp(IntPtr screen, uint attrs, short pair, IntPtr opts);
        int vid_puts(uint attrs, short pair, IntPtr opts, IntPtr OUTC);
        int vid_puts_sp(IntPtr screen, uint attrs, short pair, IntPtr opts, IntPtr OUTC);
        int vidattr(uint attrs);
        int vidattr_sp(IntPtr screen, uint attrs);
        int vidputs(uint attrs, IntPtr OUTC);
        int vidputs_sp(IntPtr screen, uint attrs, IntPtr OUTC);
        int vline(uint ch, int n);
        int vline_set(IntPtr wch, int n);
        int wadd_wch(IntPtr window, IntPtr wch);
        int wadd_wchnstr(IntPtr window, IntPtr wchStr, int n);
        int wadd_wchstr(IntPtr window, IntPtr wchStr);
        int waddch(IntPtr window, uint ch);
        int waddchnstr(IntPtr window, IntPtr txt, int number);
        int waddchstr(IntPtr window, IntPtr txt);
        int waddnstr(IntPtr window, string txt, int number);
        int waddnwstr(IntPtr window, IntPtr wstr, int n);
        int waddstr(IntPtr window, string txt);
        int waddwstr(IntPtr window, IntPtr wstr);
        int wattr_get(IntPtr window, IntPtr attrs, IntPtr pair, IntPtr opts);
        int wattr_get(IntPtr window, out uint attrs, out short pair, IntPtr opts);
        int wattr_off(IntPtr window, uint attrs, IntPtr opts);
        int wattr_on(IntPtr window, uint attrs, IntPtr opts);
        int wattr_set(IntPtr window, uint attrs, short pair, IntPtr opts);
        int wattroff(IntPtr window, int attrs);
        int wattron(IntPtr window, int attrs);
        int wattrset(IntPtr window, int attrs);
        int wbkgd(IntPtr window, uint bkgd);
        void wbkgdset(IntPtr window, uint bkgd);
        void wbkgrndset(IntPtr window, IntPtr wch);
        int wborder(IntPtr window, uint ls, uint rs, uint ts, uint bs, uint tl, uint tr, uint bl, uint br);
        int wborder_set(IntPtr window, IntPtr ls, IntPtr rs, IntPtr ts, IntPtr bs, IntPtr tl, IntPtr tr, IntPtr bl, IntPtr br);
        int wchgat(IntPtr window, int number, uint attrs, short pair, IntPtr opts);
        int wclear(IntPtr window);
        int wclrtobot(IntPtr window);
        int wclrtoeol(IntPtr window);
        int wcolor_set(IntPtr window, short pair, IntPtr opts);
        void wcursyncup(IntPtr window);
        int wdelch(IntPtr window);
        int wdeleteln(IntPtr window);
        int wecho_wchar(IntPtr window, IntPtr wch);
        int wechochar(IntPtr window, uint ch);
        bool wenclose(IntPtr window, int y, int x);
        int werase(IntPtr window);
        int wget_wch(IntPtr window, IntPtr wch);
        int wget_wstr(IntPtr window, IntPtr wstr);
        int wgetbkgrnd(IntPtr window, IntPtr wch);
        int wgetch(IntPtr window);
        int wgetdelay(IntPtr window);
        int wgetn_wstr(IntPtr window, IntPtr wstr, int n);
        int wgetnstr(IntPtr window, StringBuilder builder, int count);
        IntPtr wgetparent(IntPtr window);
        int wgetscrreg(IntPtr window, IntPtr lines, IntPtr columns);
        int wgetstr(IntPtr window, StringBuilder str);
        int whline(IntPtr window, uint ch, int count);
        int whline_set(IntPtr window, IntPtr wch, int n);
        int win_wch(IntPtr window, IntPtr wch);
        int win_wchnstr(IntPtr window, IntPtr wch, int n);
        int win_wchstr(IntPtr window, IntPtr wch);
        uint winch(IntPtr window);
        int winchnstr(IntPtr window, IntPtr txt, int count);
        int winchstr(IntPtr window, IntPtr txt);
        int winnstr(IntPtr window, StringBuilder str, int n);
        int winnwstr(IntPtr window, IntPtr str, int n);
        int wins_nwstr(IntPtr window, IntPtr str, int n);
        int wins_wch(IntPtr window, IntPtr wch);
        int wins_wstr(IntPtr window, IntPtr str);
        int winsch(IntPtr window, uint ch);
        int winsdelln(IntPtr window, int n);
        int winsertln(IntPtr window);
        int winsnstr(IntPtr window, string str, int n);
        int winsstr(IntPtr window, string str);
        int winstr(IntPtr window, StringBuilder str);
        int winwstr(IntPtr window, IntPtr str);
        bool wmouse_trafo(IntPtr win, ref int pY, ref int pX, bool to_screen);
        int wmove(IntPtr window, int y, int x);
        int wnoutrefresh(IntPtr window);
        int wprintw(IntPtr window, string format, object[] var);
        int wredrawln(IntPtr window, int beg_line, int num_lines);
        int wredrawwin(IntPtr window);
        int wrefresh(IntPtr window);
        int wresize(IntPtr window, int lines, int columns);
        int wscanw(IntPtr window, StringBuilder format, object[] var);
        int wscrl(IntPtr window, int n);
        int wsetscrreg(IntPtr window, int top, int bot);
        int wstandend(IntPtr window);
        int wstandout(IntPtr window);
        void wsyncdown(IntPtr window);
        void wsyncup(IntPtr window);
        void wtimeout(int delay);
        int wtouchln(IntPtr window, int y, int n, int changed);
        IntPtr wunctrl(IntPtr ch);
        IntPtr wunctrl_sp(IntPtr screen, IntPtr ch);
        int wvline(IntPtr window, uint ch, int n);
        int wvline_set(IntPtr window, IntPtr wch, int n);
    }
}