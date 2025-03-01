﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHARED.Interface
{
    public interface _CRUDInterface<T>
    {
        /// <summary>
        /// Thêm
        /// </summary>
        /// <returns></returns>
            int add();
        /// <summary>
        /// Sửa
        /// </summary>
        /// <returns></returns>
            int update();
        /// <summary>
        /// Xóa
        /// </summary>
        /// <returns></returns>
            int delete();
        /// <summary>
        /// Trả về Object tương đương ứng với dbContext mới
        /// </summary>
        /// <returns></returns>
            T reload();
        /// <summary>
        /// Sử dụng để FORCE LOAD FK OBJECT khi UPDATE
        /// </summary>
            void trigger();
        /// <summary>
        /// Force load all fk object
        /// </summary>
            void doTrigger();
        /// <summary>
        /// Clone object ra object khác,
        /// giữ nguyên khóa ngoại
        /// </summary>
        /// <returns></returns>
            T clone();
        /// <summary>
        /// Order column
        /// </summary>
            void moveUp();
        /// <summary>
        /// Order column
        /// </summary>
            void moveDown();
        /// <summary>
        /// Thông tin cơ bản của 1 đối tượng
        /// </summary>
        /// <returns></returns>
            String niceName();

            T prevObj();
            T nextObj();

    }
}
