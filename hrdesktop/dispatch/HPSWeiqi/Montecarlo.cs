using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HPSWeiqi
{
    public class Montecarlo
    {
        double komi = 6.5;
        const int B_SIZE = 9;
        const int WITH = B_SIZE + 2;
        const int BOARD_MAX = WITH * WITH;
        int[] board = new int[BOARD_MAX]{
            3,3,3,3,3,3,3,3,3,3,3,
            3,0,0,0,0,0,0,0,0,0,3,
            3,0,0,0,0,0,0,0,0,0,3,
            3,0,0,0,0,0,0,0,0,0,3,
            3,0,0,0,0,0,0,0,0,0,3,
            3,0,0,0,0,0,0,0,0,0,3,
            3,0,0,0,0,0,0,0,0,0,3,
            3,0,0,0,0,0,0,0,0,0,3,
            3,0,0,0,0,0,0,0,0,0,3,
            3,0,0,0,0,0,0,0,0,0,3,
            3,3,3,3,3,3,3,3,3,3,3
        };
        int[] dir4 = new int[4] { 1, -1, WITH, -WITH };
        int[] hama = new int[2];
        int[] kifu = new int[1000];
        int ko_z;
        int all_playouts;

        int get_z(int x,int y)
        {
            return (y + 1) * WITH + (x + 1);
        }

        int get81(int z)
        {
            if (z == 0) return 0;
            int y = z / WITH;
            int x = z - y * WITH;
            return x * 10 + y;
        }
        int flip_color(int col)
        {
            return 3 - col;
        }

        int select_best_move(int color)
        {
            int try_num = 50;
            int best_z = 0;
            double best_value = -100;
            int[] board_copy = new int[BOARD_MAX];
            for (int i = 0; i < BOARD_MAX; i++)
            {
                board_copy[i] = board[i];
            }
            int ko_z_copy = ko_z;
            for (int y = 0; y < B_SIZE; y++)
            {
                for (int x = 0; x < B_SIZE; x++)
                {
                    int z = get_z(x, y);
                    if (board[z] != 0) continue;
                    int err = move(z, color);
                    if (err != 0) continue;
                    int win_sum = 0;
                    for (int j = 0; j < try_num; j++)
                    {
                        int[] board_copy2 = new int[BOARD_MAX];
                        for (int i = 0; i < BOARD_MAX; i++)
                        {
                            board_copy2[i] = board[i];
                        }
                        int ko_z_copy2 = ko_z;
                        int win = -playout(flip_color(color));
                        win_sum += win;
                        for (int i = 0; i < BOARD_MAX; i++)
                        {
                            board[i] = board_copy2[i];
                        }
                        ko_z = ko_z_copy2;
                    }
                    double win_rate = (double)win_sum / try_num;
                    if (win_rate > best_value)
                    {
                        best_value = win_rate;
                        best_z = z;
                    }
                    for (int i = 0; i < BOARD_MAX; i++)
                    {
                        board[i] = board_copy[i];
                    }
                    ko_z = ko_z_copy;
                }
            }
            return best_z;
        }

        int playout(int turn_color)
        {
            all_playouts++;
            int color = turn_color;
            int befor_z = -1;
            int loop;
            int loop_max = B_SIZE * B_SIZE + 200;
            for (loop = 0; loop < loop_max; loop++)
            {
                int[] kouho = new int[BOARD_MAX];
                int kouho_num=0;
                for (int y = 0; y < B_SIZE; y++)
                {
                    for (int x = 0; x < B_SIZE; x++)
                    {
                        int z = get_z(x, y);
                        if (board[z] != 0) continue;
                        kouho[kouho_num] = z;
                        kouho_num++;
                    }
                }
                int z1, r=0;
                while (true)
                {
                    if (kouho_num == 0)
                    {
                        z1 = 0;
                    }
                    else
                    {
                        r = (new Random()).Next(kouho_num);
                        z1 = kouho[r];
                    }
                    int err = move(z1, color);
                    if (err == 0) break;
                    kouho[r] = kouho[kouho_num - 1];
                    kouho_num--;
                }
                if (z1 == 0 && befor_z == 0) break;
                befor_z = z1;
                color = flip_color(color);
            }
            return count_score(turn_color);
        }

        int count_score(int turn_color)
        {
            int score = 0;
            int[] kind = new int[3] { 0, 0, 0 };
            for (int y = 0; y < B_SIZE; y++)
            {
                for (int x = 0; x < B_SIZE; x++)
                {
                    int z = get_z(x, y);
                    int c = board[z];
                    kind[c]++;
                    if (c != 0) continue;
                    int[] mk = new int[4];
                    mk[1] = 0;
                    mk[2] = 0;
                    for (int i = 0; i < 4; i++)
                    {
                        mk[board[z + dir4[i]]]++;
                    }
                    if (mk[1] != 0 && mk[2] == 0) score++;
                    if (mk[2] != 0 && mk[1] == 0) score--;
                }
            }
            score += kind[1] - kind[2];
            double final_score = score - komi;
            int win = 0;
            if (final_score > 0) win = 1;
            if (turn_color == 2) win = -win;
            return win;
        }

        int move(int tz, int color)
        {
            if (tz == 0) 
            {
                ko_z = 0;
                return 0;
            }
            int[,] around = new int[4, 3];
            int un_col = flip_color(color);
            int space = 0;
            int kabe = 0;
            int mikata_safe = 0;
            int take_sum = 0;
            int ko_kamo = 0;
            for (int i = 0; i < 4; i++)
            {
                around[i, 0] = around[i, 1] = around[i, 2] = 0;
                int z = tz + dir4[i];
                int c = board[z];
                if (c == 0) space++;
                if (c == 3) kabe++;
                if (c == 0 || c == 3) continue;
                int dame=0;
                int ishi=0;
                count_dame(z, ref dame, ref ishi);
                around[i, 0] = dame;
                around[i, 1] = ishi;
                around[i, 2] = c;
                if (c == un_col && dame == 1)
                {
                    take_sum += ishi;
                    ko_kamo = z;
                }
                if (c == color && dame >= 2) mikata_safe++;
            }
            if(take_sum ==0 && space ==0 && mikata_safe ==0)return 1;
            if (tz == ko_z) return 2;
            if (kabe + mikata_safe == 4) return 3;
            if (board[tz] != 0) return 4;
            for (int i = 0; i < 4; i++)
            {
                int d = around[i, 0];
                int n = around[i, 1];
                int c = around[i, 2];
                if (c == un_col && d == 1 && board[tz + dir4[i]] != 0)
                {
                    kesu(tz + dir4[i], un_col);
                    hama[color - 1] += n;
                }
            }
            board[tz] = color;
            int dame2=0, ishi2=0;
            count_dame(tz, ref dame2, ref ishi2);
            if (take_sum == 1 && ishi2 == 1 && dame2 == 1)
            {
                ko_z = ko_kamo;
            }
            else
            {
                ko_z = 0;
            }
            return 0;

        }
        void kesu(int tz, int color)
        {
            int z, i;
            board[tz] = 0;
            for (i = 0; i < 4; i++)
            {
                z = tz + dir4[i];
                if (board[z] == color)
                {
                    kesu(z, color);
                }
            }
        }
        int[] check_board = new int[BOARD_MAX];
        void count_dame(int tz, ref int p_dame, ref int p_ishi)
        {
            p_dame = 0;
            p_ishi = 0;
            for (int i = 0; i < BOARD_MAX; i++) check_board[i] = 0;
            count_dame_sub(tz,board[tz],ref p_dame,ref p_ishi);
        }

        void count_dame_sub(int tz,int color, ref int p_dame, ref int p_ishi)
        {
            int z, i;
            check_board[tz] = 1;
            p_ishi++;
            for (i = 0; i < 4; i++)
            {
                z = tz + dir4[i];
                if (check_board[z] != 0) continue;
                if (board[z] == 0)
                {
                    check_board[z] = 1;
                    p_dame++;
                }
                if (board[z] == color) count_dame_sub(z, color, ref p_dame, ref p_ishi);
            }
        }

        public Montecarlo()
        {
            for (int idx = 0; idx < NODE_MAX; idx++)
            {
                node[idx] = new NODE();
                node[idx].child = new CHILD[CHILD_MAX];
            }
        }

        public bool Play(int[][] data,int color,ref int x1,ref int y1)
        {
            for (int y = 0; y < B_SIZE; y++)
            {
                for (int x = 0; x < B_SIZE; x++)
                {
                    int z = get_z(x, y);
                    board[z] = data[x][y];
                }
            }
            int z1 = select_best_move(color);
            int err = move(z1, color);
            if (err != 0)
            {
                return false;
            }
            y1 = z1 / WITH;
            x1 = z1 - y1 * WITH;

            for (int y = 0; y < B_SIZE; y++)
            {
                for (int x = 0; x < B_SIZE; x++)
                {
                    int z = get_z(x, y);
                    data[x][y] = board[z];
                }
            }
            return true;
        }
        public struct CHILD
        {
            public int z;
            public int games;
            public double rate;
            public int next;
        }
        const int CHILD_MAX = B_SIZE*B_SIZE+1;
        public struct NODE
        {            
            public int child_num;
            public CHILD[] child;
            public int games_sum;
        }
        const int NODE_MAX = 10000;
        NODE[] node = new NODE[NODE_MAX];
        int node_num = 0;
        const int NODE_EMPTY = -1;
        const int ILLEGAL_Z = -1;

        int select_best_uct(int color)
        {
            node_num = 0;
            int next = create_node();
            int uct_loop = 1000;
            for (int i = 0; i < uct_loop; i++)
            {
                int[] board_copy = new int[BOARD_MAX];
                for (int j = 0; j < BOARD_MAX; j++)
                {
                    board_copy[j] = board[j];
                }
                int ko_z_copy = ko_z;
                search_uct(color,next);
                for (int j = 0; j < BOARD_MAX; j++)
                {
                    board[j] = board_copy[j];
                }
                ko_z = ko_z_copy;
            }
            int best_i = -1;
            int max = -999;
            for (int i = 0; i < node[next].child_num; i++)
            {
                CHILD p_child = node[next].child[i];
                if (p_child.games > max)
                {
                    best_i = i;
                    max = p_child.games;
                }
            }
            int ret_z = node[next].child[best_i].z;
            return ret_z;
        }

        int create_node()
        {
            if (node_num == NODE_MAX)
            {
                return -1;
            }
            node[node_num].child_num = 0;
            for (int y = 0; y < B_SIZE; y++)
            {
                for (int x = 0; x < B_SIZE; x++)
                {
                    int z = get_z(x, y);
                    if (board[z] != 0) continue;
                    add_child(z);
                }
            }
            add_child(0);
            node_num++;
            return node_num - 1;
        }

        void add_child(int z)
        {
            int n = node[node_num].child_num;
            node[node_num].child[n].z = z;
            node[node_num].child[n].games = 0;
            node[node_num].child[n].rate = 0;
            node[node_num].child[n].next = NODE_EMPTY;
            node[node_num].child_num++;
        }
        
        const double C = 0.31;

        int search_uct(int color, int node_n)
        {
            int select = -1;
            while (true)
            {
                double max_ucb = -999;
                for (int i = 0; i < node[node_n].child_num; i++)
                {
                    if (node[node_n].child[i].z == ILLEGAL_Z) continue;
                    double ucb = 0;
                    if (node[node_n].child[i].games == 0)
                    {
                        ucb = 10000 + (new Random()).Next();
                    }
                    else
                    {
                        ucb = node[node_n].child[i].rate + C * Math.Sqrt(Math.Log(node[node_n].games_sum) / node[node_n].child[i].games);
                    }
                    if (ucb > max_ucb)
                    {
                        max_ucb = ucb;
                        select = i;
                    }
                }
                if (select == -1) return -1;
                int z = node[node_n].child[select].z;
                int err = move(z, color);
                if (err != 0)
                {
                    node[node_n].child[select].z = ILLEGAL_Z;
                    continue;
                }
                break;
            }
            int win;
            if (node[node_n].child[select].games == 0)
            {
                win = -playout(flip_color(color));
            }
            else
            {
                if (node[node_n].child[select].next == NODE_EMPTY) node[node_n].child[select].next = create_node();
                win = -search_uct(flip_color(color), node[node_n].child[select].next);
            }
            node[node_n].child[select].rate = (node[node_n].child[select].rate * node[node_n].child[select].games + win) / (node[node_n].child[select].games + 1);
            node[node_n].child[select].games++;
            node[node_n].games_sum++;
            return win;
        }
        public bool Play_UCT(int[][] data, int color, ref int x1, ref int y1)
        {
            for (int y = 0; y < B_SIZE; y++)
            {
                for (int x = 0; x < B_SIZE; x++)
                {
                    int z = get_z(x, y);
                    board[z] = data[x][y];
                }
            }
            int z1 = select_best_uct(color);
            int err = move(z1, color);
            if (err != 0)
            {
                return false;
            }
            y1 = z1 / WITH;
            x1 = z1 - y1 * WITH;

            for (int y = 0; y < B_SIZE; y++)
            {
                for (int x = 0; x < B_SIZE; x++)
                {
                    int z = get_z(x, y);
                    data[x][y] = board[z];
                }
            }
            return true;
        }

    }
}
