# -*- coding: utf-8 -*-
import sys
import os
from csharp_generate import *

reload(sys)
sys.setdefaultencoding('utf8')

if __name__ == '__main__':

    if len(sys.argv) == 1:
        print 'ERROR: please specify the dir of excel as the paramter'
    else:
        config_name_lists = ''
        config_items = ''

        csharp_generate = CSharpGenerate()

        for parent, dirnames, filenames in os.walk(sys.argv[1]):
            for filename in filenames:

                if os.path.splitext(filename)[1] != '.xls' and os.path.splitext(filename)[1] != '.xlsx':
                    continue

                # LINUX系统下判断是不是临时文件
                if filename[:2] == '~$':
                    continue

                print '===================================================='
                print filename

                full_name = os.path.join(parent, filename)

                # C#
                csharp_generate.load_xls_file(full_name)
                config_name_list, items = csharp_generate.get_config_info()
                if len(config_name_lists) > 0:
                    config_name_lists += ", "
                if len(config_name_list) > 0:
                    config_name_lists += config_name_list
                config_items += items

        # 写C#代码文件
        csharp_generate.export_config_manager(config_name_lists)
        csharp_generate.export_config_item(config_items)

        print '==========================END======================='




