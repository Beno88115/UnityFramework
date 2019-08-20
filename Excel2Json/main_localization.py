# -*- coding: utf-8 -*-
import sys
import os
# from csharp_generate import *
from localization_generate import *
from macro_generate import *

reload(sys)
sys.setdefaultencoding('utf8')

if __name__ == '__main__':

    if len(sys.argv) == 1:
        print 'ERROR: please specify the dir of excel as the paramter'
    else:
        config_manager_fields = ''
        config_manager_properties = ''
        config_manager_loads = ''
        config_items = ''

        localization_generate = LocalizationGenerate()
        macro_generate = MACROGenerate()

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

                # LOCALIZATION
                localization_generate.load_xls_file(full_name)
                localization_generate.parse_excel()
                localization_generate.export_json()

                # MACRO
                # 宏定义文件，只读取表的id及macro字段
                macro_generate.load_xls_file(full_name)
                macro_generate.export_macro()

        print '==========================END======================='




